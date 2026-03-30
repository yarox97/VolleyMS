using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Users.UpdateAttributes
{
    public record UpdatePlayerAttributesCommand(
        Guid UserId, 
        PlayerPosition? Position,
        int? HeightCm,
        double? WeightKg,
        DominantHand? DominantHand,
        int? SpikeReachCm,
        int? BlockReachCm
    ) : IRequest<Result>;
}
