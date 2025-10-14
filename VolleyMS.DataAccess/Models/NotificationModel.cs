using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.Core.Common;
using VolleyMS.DataAccess.Entities;

namespace VolleyMS.DataAccess.Models
{
    public class NotificationModel : BaseEntity
    {
        public NotificationModel()
        {
            Receivers = new List<UserModel>();
            Sender = new UserModel();
        }
        public Guid Id { get; set; }
        public NotificationType notificationType { get; set; }
        public bool isChecked { get; set; } = false;
        public string Text { get; set;  } = string.Empty;
        public string? LinkedURL { get; set; }
        public Guid senderId { get; set; }

        public IList<UserModel> Receivers { get; set; }
        public UserModel Sender { get; set; }
    }
}
