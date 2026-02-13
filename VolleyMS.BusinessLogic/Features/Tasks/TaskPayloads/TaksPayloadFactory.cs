using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.BusinessLogic.Features.Notifications.NotificationPayloads.Categorized;
using VolleyMS.BusinessLogic.Features.Tasks.TaskPayloads.Categorized;
using VolleyMS.Core.Requests;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.TaskPayloads
{
    public static class TaksPayloadFactory
    {
        public static Result<string>? CreatePayLoad(TaskType taskType)
        {
            return taskType switch
            {
                _ => null
            };
        }
    }
}
