namespace VolleyMS.Core.Requests
{
    public class TaskRequest
    {
        // BASIC TASK INFO
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskType TaskType { get; set; }
       
        // Date 
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public List<DayOfWeek>? DaysOfWeek { get; set; } // for the repeatable tasks

        // Penalty type to fine or to bonus players. 
        public PenaltyType penaltyType { get; set; } = PenaltyType.None; // Not obligatory, default value can be set to None

        // Club which task is pinned to
        public Guid ClubId { get; set; }
        public string ClubName { get; set; }

        // Sender
        public Guid SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderSurname { get; set; }

        // BASIC TASK INFO

        // Possible payload info


    }
}
