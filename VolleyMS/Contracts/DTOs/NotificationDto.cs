namespace VolleyMS.Contracts.DTOs
{
    public class NotificationDto
    {
        public string Text { get; set; } = string.Empty;
        public NotificationCategory notificationCategory { get; set; } 
        public string LinkedUrl { get; set; } = string.Empty;
        public Guid? SenderId { get; set; }
        public bool IsChecked { get; set; }

    }
}
