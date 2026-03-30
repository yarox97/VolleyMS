using MediatR;
using Microsoft.EntityFrameworkCore;
using VolleyMS.Core.DomainEvents.Tasks;
using VolleyMS.Core.Models.Payslips;
using VolleyMS.Core.Repositories;
using VolleyMS.DataAccess;

namespace VolleyMS.BusinessLogic.Features.Tasks.Confirm
{
    public class UserTaskEvaluatedDomainEventHandler : INotificationHandler<UserTaskEvaluatedDomainEvent>
    {
        private readonly VolleyMsDbContext _dbContext;

        public UserTaskEvaluatedDomainEventHandler(VolleyMsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(UserTaskEvaluatedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            // if coach did not apply penalty, we do nothing
            if (!domainEvent.ApplyPenalty || !domainEvent.PenaltyAmount.HasValue)
                return;

            var currentDate = DateTime.UtcNow;
            var currentMonth = currentDate.Month;
            var currentYear = currentDate.Year;

            // get current payslip for the user 
            var currentPayslip = await _dbContext.Payslips
                .FirstOrDefaultAsync(p =>
                    p.UserId == domainEvent.UserId &&
                    p.Status == PayslipStatus.Draft &&
                    p.Month == currentMonth &&  
                    p.Year == currentYear,       
                    cancellationToken);

            if (currentPayslip == null)
            {
                // We need the active contract to create a payslip (to know base salary & currency)
                // Assuming a user has only one active contract per club, we need to find the correct one.
                // Since the DomainEvent has TaskId, we can fetch the task to know the ClubId.

                var task = await _dbContext.Tasks
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == domainEvent.TaskId, cancellationToken);

                if (task == null)
                    throw new InvalidOperationException($"Task {domainEvent.TaskId} not found for penalty application.");

                var contract = await _dbContext.Contracts
                .FirstOrDefaultAsync(c =>
                    c.UserId == domainEvent.UserId &&
                    c.ClubId == task.ClubId &&
                    (c.EndsBy == null || c.EndsBy >= currentDate), 
                    cancellationToken);

                if (contract == null || contract.MonthlySalary == null)
                {
                    // If no contract, we can't apply a financial penalty to a payslip
                    throw new InvalidOperationException($"Active contract not found for user {domainEvent.UserId} in club {task.ClubId}. Cannot create payslip for fine.");
                }

                var baseSalaryFromContract = (decimal)contract.MonthlySalary.Value;

                // Create new Payslip
                var newPayslip = Payslip.Create(
                    userId: domainEvent.UserId,
                    contractId: contract.Id,
                    month: currentMonth,
                    year: currentYear,
                    baseSalary: baseSalaryFromContract
                );

                if(newPayslip.IsFailure)
                {
                    throw new Exception($"Failed to create payslip for user {domainEvent.UserId}: {newPayslip.Error.Message}");
                }

                currentPayslip = newPayslip.Value;

                // Add to DbContext tracking
                _dbContext.Payslips.Add(currentPayslip);
            }

            // penalty type determines the adjustment type
            var adjType = AdjustmentType.PenaltyValue; // default to percent penalty

            switch (domainEvent.PenaltyType)
            {
                case PenaltyType.None:
                    return;
                case PenaltyType.PercentPremia:
                    adjType = AdjustmentType.BounsPercent;
                    break;
                case PenaltyType.ValuePremia:
                    adjType = AdjustmentType.BonusValue;
                    break;
                case PenaltyType.PercentFine:
                    adjType = AdjustmentType.PenaltyPercent;
                    break;
                case PenaltyType.ValueFine:
                    adjType = AdjustmentType.PenaltyValue;
                    break;
                default:
                    throw new ArgumentException($"Unknown penalty type: {domainEvent.PenaltyType}");
            }

            // add adjustment to the payslip
            var addResult = currentPayslip.AddAdjustment(
                amount: domainEvent.PenaltyAmount.Value,
                type: adjType,
                reason: domainEvent.Reason,
                sourceTaskId: domainEvent.TaskId
            );

            if (addResult.IsFailure)
            {
                throw new Exception(addResult.Error.Message);
            }

        }
    }
}
