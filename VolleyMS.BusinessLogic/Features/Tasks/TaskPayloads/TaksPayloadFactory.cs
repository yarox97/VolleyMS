using System.Text.Json;
using VolleyMS.BusinessLogic.Features.Tasks.TaskPayloads.Categorized;
using VolleyMS.Core.Shared;
using TaskRequest = VolleyMS.Core.Requests.TaskRequest;

namespace VolleyMS.BusinessLogic.Features.Tasks.TaskPayloads
{
    public static class TaksPayloadFactory
    {
        public static Result<string>? CreatePayLoad(TaskRequest taskRequest, Guid creatorId)
        {
            return taskRequest.TaskType switch
            {
                TaskType.Survey => JsonSerializer.Serialize(new TaskTypeSurveyPayload
                {
                    surveyUrl = taskRequest.SurveyURL ?? throw new ArgumentException("SurveyURL is required for Survey tasks")
                }),

                _ => null
            };
        }
    }
}
