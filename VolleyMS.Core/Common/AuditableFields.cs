namespace VolleyMS.Core.Common
{
    public class AuditableFields
    {
        public AuditableFields()
        {
            CreatedAt = DateTime.UtcNow;
        }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
