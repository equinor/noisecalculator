using System.Collections.Generic;

namespace NoiseCalculator.Domain.Entities
{
    public class HelicopterNoiseProtectionDefinition
    {
        public virtual int Id { get; set; }
        public virtual string SystemName { get; set; }
        public virtual IList<HelicopterNoiseProtection> HelicopterNoiseProtections { get; set; }
    }
}