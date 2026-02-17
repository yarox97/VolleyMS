using Microsoft.AspNetCore.Http;

namespace VolleyMS.Core.Requests
{
    public class TaskRequest
    {
        // BASIC TASK INFO
        public required string Title { get; set; } 
        public required string Description { get; set; } =  string.Empty;
        public TaskType TaskType { get; set; }
        public TaskPriority TaskPriority { get; set; }

        // Receivers
        public IEnumerable<Guid> Receivers { get; set; } = new List<Guid>();

        // Date 
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public List<DayOfWeek>? DaysOfWeek { get; set; } // for the repeatable tasks

        // Penalty type to fine or to bonus players. 
        public PenaltyType penaltyType { get; set; } = PenaltyType.None; // Not obligatory, default value can be set to None

        // Files (optional)
        public ICollection<IFormFile>? Files { get; set; }

        // Club which task is pinned to
        public Guid ClubId { get; set; }
        public string ClubName { get; set; }

        // Sender
        public string SenderName { get; set; }
        public string SenderSurname { get; set; }

        // BASIC TASK INFO



        // Possible payload info |
        //                       V
        public string? ConfirmationButtonText { get; set; }

        // Match
        public string? OpponentName { get; set; } 
        public bool? IsHomeGame { get; set; } 
        public string? HallAddress { get; set; } 
        public string? GatheringTime { get; set; } 
        public string? DepartureTime { get; set; }
        public string? GoogleMapsLink { get; set; } 

        // Survey
        public string? SurveyURL { get; set; } // For survey tasks

        

    }
}
