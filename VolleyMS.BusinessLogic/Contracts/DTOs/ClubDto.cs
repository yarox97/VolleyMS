namespace VolleyMS.BusinessLogic.Contracts.DTOs
{
    public class ClubDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? JoinCode { get; set; }
        public string Description { get; set; } = string.Empty;
        public string AvatarURL { get; set; } = string.Empty;
        public string BackGroundURL { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
    }
}
