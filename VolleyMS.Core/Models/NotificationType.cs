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

        public (NotificationType, string) Create(Guid Id, NotificationCategory notificationCategory, IList<ClubMemberRole> requiredClubMemberRole)
        {
            string error = string.Empty;
            if (!Enum.IsDefined(typeof(NotificationCategory), notificationCategory))
            {
                error = "Invalid category";
            }
            if (!Enum.IsDefined(typeof(ClubMemberRole), requiredClubMemberRole))
            {
                error = "Invalid required role";
            }

            return (new NotificationType(Id, notificationCategory, requiredClubMemberRole), error);
        }
    }
}
