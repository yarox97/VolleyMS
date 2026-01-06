using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Users.Authorisation.LoginCheck
{
    public sealed record LoginCheckQuery(string login) : IRequest<Result<bool>>;
}
