namespace VolleyMS.BusinessLogic.Features.Tasks.TaskPayloads.Categorized
{
    public class TaskTypeMatchPayload
    {
        public required string OpponentName { get; set; }
        public bool IsHomeGame { get; set; }

        public required string HallAddress { get; set; }
        public string? GoogleMapsLink { get; set; }

        public required string GatheringTime { get; set; } 
        public string? DepartureTime { get; set; }
    }
}
