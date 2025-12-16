namespace VolleyMS.BusinessLogic.Features.Notifications.Get
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        // Required fields
        public string Text { get; set; }
        public bool IsChecked { get; set; }
        public string? LinkedURL { get; set; }
        public NotificationCategory NotificationCategory { get; set; }
        public IList<ClubMemberRole> RequiredClubMemberRoles { get; set; }
        public Guid? SenderId { get; set; }
        public string? Payload { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
