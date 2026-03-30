namespace VolleyMS.BusinessLogic.Features.Users.UpdateAttributes
{
    public record UpdateAttributesDto(
        PlayerPosition? Position,
        int? HeightCm,
        double? WeightKg,
        DominantHand? DominantHand,
        int? SpikeReachCm,
        int? BlockReachCm
    );
}
