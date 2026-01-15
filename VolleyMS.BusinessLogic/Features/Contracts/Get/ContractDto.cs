namespace VolleyMS.BusinessLogic.Features.Contracts.Get
{
    public class ContractDto
    {
        // Contract details!!
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal? Salary { get; set; }
        public Currency? Currency { get; set; }
        public DateTime AssignedAt { get; set; }
        public bool IsActive { get; set; }

        // Club details
        public string ClubName { get; set; } = string.Empty;
        public Guid ClubId { get; set; }
        public string? ClubUrl => $"api/clubs/{ClubId}";

        //Member details
        public Guid MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public string MemberSurname { get; set; } = string.Empty;

        // Assigner details
        public string ResponserName { get; set; } = string.Empty;
        public string ResponserSurname { get; set; } = string.Empty;
        public string ResponserUserName { get; set; } = string.Empty;
        public string? ResponserUrl => $"api/users/{ResponserUserName}";

    }
}
