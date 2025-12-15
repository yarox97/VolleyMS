using MediatR;
using VolleyMS.BusinessLogic.Contracts.DTOs;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Users.Get
{
    public sealed record GetUserQuery(string userName, Guid requestorId) : IRequest<Result<UserDto>>
    {
    }
}
