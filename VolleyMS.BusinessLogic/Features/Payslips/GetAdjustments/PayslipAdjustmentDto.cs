namespace VolleyMS.BusinessLogic.Features.Payslips.GetAdjustments
{
    public class PayslipAdjustmentDto
    {
        public Guid Id { get; set; }
        public Guid PayslipId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public Guid? SourceTaskId { get; set; }
        public DateTime Date { get; set; }
        public bool IsReverted { get; set; }
    }
}
