using VolleyMS.Core.Common;

namespace VolleyMS.Core.Models.Payslips
{
    public class PayslipAdjustment : BaseEntity
    {
        public Guid PayslipId { get; private set; }
        public Payslip Payslip { get; private set; }

        public decimal Amount { get; private set; }
        public AdjustmentType Type { get; private set; }
        public string Reason { get; private set; }
        public Guid? SourceTaskId { get; private set; }
        public DateTime Date { get; private set; }
        public bool IsReverted { get; private set; }

        private PayslipAdjustment() : base(Guid.Empty)
        {
        }

        internal PayslipAdjustment(
            Guid id,
            Payslip payslip,
            decimal amount,
            AdjustmentType type,
            string reason,
            Guid? sourceTaskId,
            bool isReverted) : base(id)
        {
            Payslip = payslip;
            PayslipId = payslip.Id;
            Amount = amount;
            Type = type;
            Reason = reason;
            SourceTaskId = sourceTaskId;
            Date = DateTime.UtcNow;
            IsReverted = isReverted;
        }

        public void Revert()
        {
            if (IsReverted)
                return;
            IsReverted = true;
        }
    }
}