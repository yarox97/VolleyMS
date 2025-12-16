using MediatR;
using VolleyMS.BusinessLogic.Features.Users.Get;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Users.Get
{
    public sealed record GetUserQuery(string userName, Guid? requestorId) : IRequest<Result<UserDto>>
    {
    }
}
