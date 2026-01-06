using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Users.Authorisation.EmailCheck
{
    public sealed record EmailCheckQuery(string email) : IRequest<Result<bool>>;
}
