using VolleyMS.Core.Common;
using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Models.Payslips
{
    public class Payslip : BaseEntity
    {
        private readonly List<PayslipAdjustment> _adjustments = new();

        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public Guid ContractId { get; private set; }
        public Contract Contract { get; private set; } 

        public int Month { get; private set; }
        public int Year { get; private set; }

        public decimal BaseSalary { get; private set; }
        public PayslipStatus Status { get; private set; }

        public IReadOnlyCollection<PayslipAdjustment> Adjustments => _adjustments.AsReadOnly();

        public decimal TotalAmount
        {
            get
            {
                var fixedBonuses = _adjustments
                    .Where(a => a.Type == AdjustmentType.BonusValue && !a.IsReverted)
                    .Sum(a => a.Amount);

                var fixedPenalties = _adjustments
                    .Where(a => a.Type == AdjustmentType.PenaltyValue && !a.IsReverted)
                    .Sum(a => a.Amount);

                var bonusPercentSum = _adjustments
                    .Where(a => a.Type == AdjustmentType.BounsPercent && !a.IsReverted)
                    .Sum(a => a.Amount);

                var penaltyPercentSum = _adjustments
                    .Where(a => a.Type == AdjustmentType.PenaltyPercent && !a.IsReverted)
                    .Sum(a => a.Amount);

                var percentBonusesAmount = BaseSalary * (bonusPercentSum / 100m);
                var percentPenaltiesAmount = BaseSalary * (penaltyPercentSum / 100m);

                var total = BaseSalary + fixedBonuses - fixedPenalties + percentBonusesAmount - percentPenaltiesAmount;

                return total < 0 ? 0 : total;
            }
        }
        private Payslip() : base(Guid.Empty)
        {
        }

        private Payslip(
            Guid id,
            Guid userId,
            Guid contractId,
            int month,
            int year,
            decimal baseSalary) : base(id)
        {
            UserId = userId;
            ContractId = contractId;
            Month = month;
            Year = year;
            BaseSalary = baseSalary;
            Status = PayslipStatus.Draft;
        }

        public static Result<Payslip> Create(
            Guid userId,
            Guid contractId,
            int month,
            int year,
            decimal baseSalary)
        {
            if (baseSalary < 0)
                return Result.Failure<Payslip>(new Error("Payslip.InvalidSalary", "Base salary cannot be negative"));

            if (month < 1 || month > 12)
                return Result.Failure<Payslip>(new Error("Payslip.InvalidMonth", "Month must be between 1 and 12"));

            var payslip = new Payslip(Guid.NewGuid(), userId, contractId, month, year, baseSalary);
            return Result.Success(payslip);
        }

        public Result AddAdjustment(decimal amount, AdjustmentType type, string reason, Guid? sourceTaskId = null)
        {
            if (amount <= 0)
                return Result.Failure(new Error("Adjustment.InvalidAmount", "Adjustment amount must be greater than zero"));

            if (Status != PayslipStatus.Draft)
                return Result.Failure(new Error("Payslip.NotEditable", "Cannot add adjustments to an approved or paid payslip"));

            if (string.IsNullOrWhiteSpace(reason))
                return Result.Failure(new Error("Adjustment.ReasonEmpty", "Reason for adjustment must be provided"));

            var adjustment = new PayslipAdjustment(Guid.NewGuid(), this, amount, type, reason, sourceTaskId, isReverted: false);
            _adjustments.Add(adjustment);

            return Result.Success();
        }

        public Result RevertAdjustment(Guid adjustmentId)
        {
            if (Status != PayslipStatus.Draft)
                return Result.Failure(new Error("Payslip.NotEditable", "Cannot revert adjustments for this status"));

            var adjustment = _adjustments.FirstOrDefault(a => a.Id == adjustmentId);
            if (adjustment == null)
                return Result.Failure(new Error("Adjustment.NotFound", "Adjustment not found"));

            if (adjustment.IsReverted)
                return Result.Failure(new Error("Adjustment.AlreadyReverted", "Adjustment is already reverted"));

            adjustment.Revert();

            return Result.Success();
        }

        public Result Approve()
        {
            if (Status != PayslipStatus.Draft)
                return Result.Failure(new Error("Payslip.InvalidState", "Only draft payslips can be approved"));

            Status = PayslipStatus.Approved;
            return Result.Success();
        }

        public Result MarkAsPaid()
        {
            if (Status != PayslipStatus.Draft)
                return Result.Failure(new Error("Payslip.InvalidState", "Payslip must be approved before it can be marked as paid"));

            Status = PayslipStatus.Paid;
            return Result.Success();
        }
    }
}