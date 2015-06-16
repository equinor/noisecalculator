
using System.Dynamic;

namespace NoiseCalculator.Domain.Entities
{
    public class HelicopterTask
    {
        public virtual int Id { get; set; }
        public virtual HelicopterType HelicopterType { get; set; }
        public virtual NoiseProtectionDefinition NoiseProtectionDefinition { get; set; }
        public virtual Task Task { get; set; }
        public virtual decimal NoiseLevel { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1} - {2}", HelicopterType.Title, NoiseProtectionDefinition.SystemName, Task.Title);
        }
    }
}
