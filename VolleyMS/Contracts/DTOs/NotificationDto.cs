namespace VolleyMS.Contracts.DTOs
{
    public class NotificationDto
    {
        // Required fields
        public string Text { get; set; }
        public bool IsChecked { get; set; }
        public string? LinkedURL { get; set; }
        public NotificationCategory NotificationCategory { get; set; }
        public IList<ClubMemberRole> RequiredClubMemberRoles { get; set; }
        public Guid? SenderId { get; set; }
        public string? PayLoad { get; set; }
    }
}
