using VolleyMS.Core.Common;
using VolleyMS.Core.Models;

namespace VolleyMS.Core.Entities
{
    public class Notification : BaseEntity
    {
        private Notification(Guid id, NotificationType notificationType, bool isChecked, string Text, string? LinkedURL)
        {
            Id = id;
            this.NotificationType = notificationType;
            this.IsChecked = isChecked;
            this.Text = Text;
        }
        public Guid Id { get; }
        public NotificationType NotificationType { get; }
        public bool IsChecked { get; } = false;
        public string Text { get; }
        public string? LinkedURL { get; } 
        

        public static (Notification notification, string error) Create(Guid id, NotificationType notificationType, bool isChecked, string Text, string? LindkedURL)
        {
            var error = string.Empty;

            if(Text == string.Empty)
            {
                error = "Notification text cannot be empty!";    
            }

            var notification = new Notification(id, notificationType, isChecked, Text, LindkedURL);
            return (notification, error);
        }
    }
}
