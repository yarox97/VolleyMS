namespace VolleyMS.Core.Requests
{
    public class NotificationRequest
    {
        // Required fields
        public bool IsChecked { get; set; } = false;
        public string Text { get; set; } = string.Empty;
        public string? LinkedURL { get; set; }
        public NotificationCategory NotificationCategory { get; set; }
        public IList<ClubMemberRole>? RequiredClubMemberRole { get; set; } = new List<ClubMemberRole>();
        public IList<Guid> Receivers { get; set; } = new List<Guid>();


        //                        |
        // Payloads possible info v

        // Sender
        public string? SenderSurname { get; set; }
        public string? SenderName { get; set; }
        public string? SenderUserName { get; set; }    

        // Tasks
        public Guid? TaskId { get; set; }
        public DateTime? TaskDueDate { get; set; }
        public string? TaskPriority { get; set; }

        // Events
        public Guid? EventId { get; set; }
        public string? EventLocation { get; set; }
        public DateTime? EventStartTime { get; set; }

        // Comments
        public Guid? CommentId { get; set; }
        public Guid? MentionedByUserId { get; set; }

        // In club notifications
        public Guid? ClubId { get; set; }
        public string? ClubName { get; set; }
        
    }
}
