using System.Text.Json;
using VolleyMS.BusinessLogic.Features.Tasks.TaskPayloads.Categorized;
using VolleyMS.Core.Shared;
using TaskRequest = VolleyMS.Core.Requests.TaskRequest;

namespace VolleyMS.BusinessLogic.Features.Tasks.TaskPayloads
{
    public static class TaksPayloadFactory
    {
        public static Result<string>? CreatePayLoad(TaskRequest req, Guid creatorId)
        {
            try
            {
                object? payloadData = req.TaskType switch
                {
                    // Info
                    TaskType.Info => new TaskTypeInfoPayload
                    {
                        ConfirmationButtonText = req.ConfirmationButtonText ?? "Mark as read"
                    },

                    // FileConfirmation
                    TaskType.DocumentSign => new TaskTypeDocumentPayload
                    {
                        ConfirmationButtonText = req.ConfirmationButtonText ?? "Confirm",
                    },

                    // Match
                    TaskType.Match => new TaskTypeMatchPayload
                    {
                        OpponentName = req.OpponentName ?? "TBD",
                        IsHomeGame = req.IsHomeGame ?? false,
                        HallAddress = req.HallAddress ?? "Addres was not given",

                        GatheringTime = req.GatheringTime ?? req.StartTime.Subtract(TimeSpan.FromHours(1)).ToString(@"hh\:mm"),

                        DepartureTime = (!req.IsHomeGame.GetValueOrDefault()) ? req.DepartureTime : null,
                        GoogleMapsLink = req.GoogleMapsLink
                    },

                    TaskType.Survey => new TaskTypeSurveyPayload
                    { 
                        surveyUrl = req.SurveyURL ?? throw new Exception("Survey URL is required for survey tasks.")
                    },

                    _ => null
                };

                if (payloadData == null) return Result.Success<string>(null!);

                return Result.Success(JsonSerializer.Serialize(payloadData));
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(new Error("PayloadFactory.Error", ex.Message));
            }
        }
    }
}