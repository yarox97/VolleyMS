namespace VolleyMS.BusinessLogic.Contracts.DTOs
{
    public class UserClubDto
    {
        public string ClubName { get; set; } = string.Empty;
        public Guid ClubId { get; set; }
        public Guid UserId { get; set; }
        public ClubMemberRole Role { get; set; }
        public DateTime JoinDate { get; set; }
    }
}