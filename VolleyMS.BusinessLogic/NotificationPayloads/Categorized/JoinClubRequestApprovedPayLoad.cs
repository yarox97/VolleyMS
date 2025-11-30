using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyMS.BusinessLogic.NotificationPayloads.Categorized
{
    public record JoinClubRequestApprovedPayLoad(Guid ClubId, string ClubName);
}
