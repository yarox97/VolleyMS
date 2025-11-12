using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.Core.Common;

namespace VolleyMS.Core.Models
{
    public class NotificationType
    {
        private NotificationType(Guid Id, NotificationCategory notificationCategory, IList<ClubMemberRole> requiredClubMemberRole)
        {
            this.Id = Id;
            this.notificationCategory = notificationCategory;
            this.requiredClubMemberRole = requiredClubMemberRole;
        }

        public Guid Id;
        public NotificationCategory notificationCategory = NotificationCategory.Informative;
        public IList<ClubMemberRole> requiredClubMemberRole;

        public static (NotificationType notificationType, string error) Create(Guid Id, NotificationCategory notificationCategory, IList<ClubMemberRole> requiredClubMemberRole)
        {
            string error = string.Empty;
            if (!Enum.IsDefined(typeof(NotificationCategory), notificationCategory))
            {
                error = "Invalid category";
            }
            foreach (var role in requiredClubMemberRole)
            {
                if (!Enum.IsDefined(typeof(ClubMemberRole), role))
                {
                    error = "Invalid required role";
                    break; 
                }
            }

            return (new NotificationType(Id, notificationCategory, requiredClubMemberRole), error);
        }
    }
}
