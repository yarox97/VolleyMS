using VolleyMS.BusinessLogic.Features.Users.Get;

namespace VolleyMS.BusinessLogic.Features.JoinClub.GetRequests
{
    public sealed record JoinClubRequestDto(
        UserDto requestor, 
        JoinClubRequestStatus joinClubRequestStatus, 
        DateTime createdAt, 
        DateTime? respondedAt, 
        UserDto responder);
}
