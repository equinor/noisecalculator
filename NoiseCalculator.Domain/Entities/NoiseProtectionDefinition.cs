using System.Collections.Generic;

namespace NoiseCalculator.Domain.Entities
{
    public class NoiseProtectionDefinition
    {
        public virtual int Id { get; set; }
        public virtual string SystemName { get; set; }
        public virtual IList<NoiseProtection> NoiseProtections { get; set; }
    }
}