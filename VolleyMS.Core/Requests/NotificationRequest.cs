using VolleyMS.Core.Models;

namespace VolleyMS.Core.Requests
{
    public class NotificationRequest
    {
        public NotificationCategory NotificationCategory { get; set; }
        public IList<ClubMemberRole> RequiredClubMemberRole { get; set; } = new List<ClubMemberRole>();
        public bool IsChecked { get; set; } = false;
        public string Text { get; set; } = string.Empty;
        public string? LinkedURL;
        public IList<Guid> Receivers { get; set; } = new List<Guid>();
    }
}