using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using VolleyMS.Core.Models.Payslips;
using VolleyMS.DataAccess;

namespace VolleyMS.BusinessLogic.BackgorundJobs
{
    [DisallowConcurrentExecution]
    public class DailyPayslipGeneratorJob : IJob
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DailyPayslipGeneratorJob> _logger;

        public DailyPayslipGeneratorJob(IServiceScopeFactory scopeFactory, ILogger<DailyPayslipGeneratorJob> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<VolleyMsDbContext>();

            var targetDate = DateTime.UtcNow;
            var month = targetDate.Month;
            var year = targetDate.Year;

            try
            {
                var activeContracts = await dbContext.Contracts
                    .Where(c =>
                        c.BeginsFrom <= targetDate &&
                        (c.EndsBy == null || c.EndsBy >= targetDate) &&
                        c.MonthlySalary != null)
                    .ToListAsync(context.CancellationToken);

                if (!activeContracts.Any()) return;

                var existingPayslipContractIds = await dbContext.Payslips
                    .Where(p => p.Month == month && p.Year == year)
                    .Select(p => p.ContractId)
                    .ToListAsync(context.CancellationToken);

                var existingSet = new HashSet<Guid>(existingPayslipContractIds);

                int generatedCount = 0;

                foreach (var contract in activeContracts)
                {
                    if (!existingSet.Contains(contract.Id))
                    {
                        var payslipResult = Payslip.Create(
                            userId: contract.UserId,
                            contractId: contract.Id,
                            month: month,
                            year: year,
                            baseSalary: contract.MonthlySalary.Value
                        );

                        if (payslipResult.IsSuccess)
                        {
                            dbContext.Payslips.Add(payslipResult.Value);
                            existingSet.Add(contract.Id);
                            generatedCount++;
                        }
                    }
                }

                if (generatedCount > 0)
                {
                    await dbContext.SaveChangesAsync(context.CancellationToken);
                    _logger.LogInformation($"Generated {generatedCount} payslips for {month}/{year}.");
                }
                else
                {
                    _logger.LogInformation("No new payslip to generate.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while createing payslip!");
                throw new JobExecutionException(ex);
            }
        }
    }
}