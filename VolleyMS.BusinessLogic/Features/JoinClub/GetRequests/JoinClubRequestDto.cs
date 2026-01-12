using VolleyMS.BusinessLogic.Features.Users.Get;

namespace VolleyMS.BusinessLogic.Features.JoinClub.GetRequests
{
    public sealed record JoinClubRequestDto(
        Guid ClubId,
        string ClubName,
        UserDto requestor, 
        JoinClubRequestStatus joinClubRequestStatus, 
        DateTime createdAt, 
        DateTime? respondedAt, 
        UserDto responder,
        Guid JoinClubRequestId);
}
