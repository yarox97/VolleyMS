using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.Core.Common;
using VolleyMS.Core.Models;
using VolleyMS.DataAccess.Entities;

namespace VolleyMS.DataAccess.Models
{
    public class NotificationEntity : BaseEntity
    {
        public NotificationEntity()
        {
            Receivers = new List<UserEntity>();
            Sender = new UserEntity();
            notificationType = new NotificationTypeEntity();
        }
        public Guid Id { get; set; }
        public bool isChecked { get; set; } = false;
        public string Text { get; set;  } = string.Empty;
        public string? LinkedURL { get; set; }
        public Guid senderId { get; set; }

        public NotificationTypeEntity notificationType { get; set; }
        public IList<UserEntity> Receivers { get; set; }
        public UserEntity Sender { get; set; }
    }
}
