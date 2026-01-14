using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Users.ChangeEmail
{
    public sealed record ChangeEmailCommand(Guid userId, string? email) : IRequest<Result>;
}
