namespace VolleyMS.BusinessLogic.Features.ClubMembers.Get
{
    public sealed record ClubMemberDto 
    {
        public Guid UserId { get; init; }
        public string UserName { get; init; } = null!;
        public string UserFirstName { get; init; } = null!;
        public string UserSurname { get; init; } = null!;
        public ClubMemberRole Role { get; init; }
        public DateTime JoinDate { get; init; }
        public string? AvatarUrl { get; init; }
    }
}