using VolleyMS.Core.Shared;

namespace VolleyMS.Core.ValueObjects
{
    public sealed class PlayerAttributes : ValueObject
    {
        public PlayerPosition? Position { get; private set; }
        public int? HeightCm { get; private set; }
        public double? WeightKg { get; private set; }
        public DominantHand? DominantHand { get; private set; }
        public int? SpikeReachCm { get; private set; }
        public int? BlockReachCm { get; private set; } // Pola atrybutów zawodnika
        private PlayerAttributes() { }

        private PlayerAttributes(
            PlayerPosition? position,
            int? heightCm,
            double? weightKg,
            DominantHand? dominantHand,
            int? spikeReachCm,
            int? blockReachCm)
        {
            Position = position;
            HeightCm = heightCm;
            WeightKg = weightKg;
            DominantHand = dominantHand;
            SpikeReachCm = spikeReachCm;
            BlockReachCm = blockReachCm;
        } // Konstruktory
        public static Result<PlayerAttributes> Create(
            PlayerPosition? position,
            int? heightCm,
            double? weightKg,
            DominantHand? dominantHand,
            int? spikeReachCm,
            int? blockReachCm)
        {
            if (heightCm.HasValue && (heightCm <= 50 || heightCm > 250))
                return Result.Failure<PlayerAttributes>(new Error("PlayerAttribute.IncorrectHeight", "Incorrect height was provided"));

            if (weightKg.HasValue && (weightKg <= 20 || weightKg > 200))
                return Result.Failure<PlayerAttributes>(new Error("PlayerAttribute.IncorrectWeight", "Incorrect weight was provided"));

            if (spikeReachCm.HasValue && (spikeReachCm <= 100 || spikeReachCm > 400))
                return Result.Failure<PlayerAttributes>(new Error("PlayerAttribute.IncorrectReach", "Incorrect reach was provided"));

            if (blockReachCm.HasValue && (blockReachCm <= 100 || blockReachCm > 400))
                return Result.Failure<PlayerAttributes>(new Error("PlayerAttribute.IncorrectReach", "Incorrect reach was provided"));

            var attributes = new PlayerAttributes(position, heightCm, weightKg, dominantHand, spikeReachCm, blockReachCm);

            return Result.Success(attributes);
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Position;
            yield return HeightCm;
            yield return WeightKg;
            yield return DominantHand;
            yield return SpikeReachCm;
            yield return BlockReachCm;
        }
    }
}
