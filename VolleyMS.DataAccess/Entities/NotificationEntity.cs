using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.Core.Common;
using VolleyMS.DataAccess.Entities;

namespace VolleyMS.DataAccess.Models
{
    public class NotificationEntity : BaseEntity
    {
        public NotificationEntity()
        {
            Receivers = new List<UserEntity>();
            Sender = new UserEntity();
        }
        public Guid Id { get; set; }
        public NotificationType notificationType { get; set; }
        public bool isChecked { get; set; } = false;
        public string Text { get; set;  } = string.Empty;
        public string? LinkedURL { get; set; }
        public Guid senderId { get; set; }

        public IList<UserEntity> Receivers { get; set; }
        public UserEntity Sender { get; set; }
    }
}
