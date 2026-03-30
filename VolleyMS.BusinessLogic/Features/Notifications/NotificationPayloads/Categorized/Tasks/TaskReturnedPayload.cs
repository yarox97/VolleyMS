using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyMS.BusinessLogic.Features.Notifications.NotificationPayloads.Categorized.Tasks
{
    internal class TaskReturnedPayload(
        DateTime StartDateTime,
        DateTime EndDateTime,
        string TaskPriority,
        string TaskTitle,
        string Description,
        string TaskPenaltyValue,
        string TaskPenaltyType,
        string ClubName,
        string AssignerFullName,
        Guid TaskId)
    {
    }
}
