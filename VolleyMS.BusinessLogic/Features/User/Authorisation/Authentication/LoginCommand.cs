using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Users.Authorisation.Authentication
{
    public record LoginCommand(string userName, string password) : IRequest<Result<string>>;
}
