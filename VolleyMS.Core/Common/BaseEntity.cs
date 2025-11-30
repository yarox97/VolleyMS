namespace VolleyMS.Core.Common
{
    public class BaseEntity : AuditableFields
    {
        protected BaseEntity(Guid id)
        {
            Id = id; 
        }
        public Guid Id { get; init; }
        public Guid? CreatorId { get; set; }
    }
}
