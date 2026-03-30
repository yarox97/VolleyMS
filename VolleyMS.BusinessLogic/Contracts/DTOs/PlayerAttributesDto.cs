using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyMS.BusinessLogic.Contracts.DTOs
{
    public class PlayerAttributesDto
    {
        public string? Position { get; set; }
        public int? HeightCm { get; set; }
        public double? WeightKg { get; set; }
        public string? DominantHand { get; set; }
        public int? SpikeReachCm { get; set; }
        public int? BlockReachCm { get; set; }
    }
}
