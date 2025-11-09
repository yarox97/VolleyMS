using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyMS.DataAccess.Entities
{
    public class NotificationTypeEntity
    {
        public NotificationTypeEntity() 
        {
            requiredClubMemberRole = new List<ClubMemberRole>();
        }

        public Guid Id { get; set; }
        public NotificationCategory notificationCategory { get; set; } = NotificationCategory.Informative;
        public IList<ClubMemberRole> requiredClubMemberRole { get; set; }
    }
}
