using MediatR;
using VolleyMS.Core.Models;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Club.Create
{
    public record CreateClubCommand : IRequest<Result<Guid>>
    {
        public Guid CreatorId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AvatarURL { get; set; } = string.Empty;
        public string BackGroundURL { get; set; } = string.Empty;
    }
}
