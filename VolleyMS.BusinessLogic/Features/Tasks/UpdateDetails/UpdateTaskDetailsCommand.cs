using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.UpdateDetails
{
    public sealed record UpdateTaskDetailsCommand(
        Guid TaskId,
        string Title,
        string Description,
        TaskType TaskType,       
        TaskPriority TaskPriority,
        PenaltyType PenaltyType,
        decimal? PenaltyAmount,

        // Расписание
        DateTime StartDate,
        DateTime? EndDate,
        TimeSpan StartTime,
        TimeSpan? EndTime,
        List<DayOfWeek>? DaysOfWeek,

        string? ConfirmationButtonText,
        string? OpponentName,
        bool? IsHomeGame,
        string? HallAddress,
        string? GatheringTime,
        string? DepartureTime,
        string? GoogleMapsLink,
        string? SurveyURL,

        Guid requestorId
    ) : IRequest<Result>
    {
        public Core.Requests.TaskRequest ToTaskRequest()
        {
            return new Core.Requests.TaskRequest
            {
                Title = this.Title,
                Description = this.Description,
                TaskType = this.TaskType,
                TaskPriority = this.TaskPriority,
                penaltyType = this.PenaltyType,
                PenaltyAmount = this.PenaltyAmount,

                StartDate = this.StartDate,
                EndDate = this.EndDate,
                StartTime = this.StartTime,
                EndTime = this.EndTime,
                DaysOfWeek = this.DaysOfWeek,

                // payloads
                ConfirmationButtonText = this.ConfirmationButtonText,
                OpponentName = this.OpponentName,
                IsHomeGame = this.IsHomeGame,
                HallAddress = this.HallAddress,
                GatheringTime = this.GatheringTime,
                DepartureTime = this.DepartureTime,
                GoogleMapsLink = this.GoogleMapsLink,
                SurveyURL = this.SurveyURL

            };
        }
    }
}
