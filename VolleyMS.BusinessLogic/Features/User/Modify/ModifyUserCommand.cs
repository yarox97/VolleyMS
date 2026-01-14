using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Users.Modify
{
    public sealed record ModifyUserCommand(Guid userId, string? Name, string? Surname) : IRequest<Result>;
}
