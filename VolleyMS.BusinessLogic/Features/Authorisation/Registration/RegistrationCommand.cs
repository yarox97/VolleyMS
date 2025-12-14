using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Authorisation.Registration
{
    public record RegistrationCommand : IRequest<Result<Guid>>
    {
        public string userName { get; init; }
        public string password { get; init; }
        public string name { get; init; }
        public string surname { get; init; }
        public string email { get; init; }
        public string? avatarUrl { get; init; }
    }
}
