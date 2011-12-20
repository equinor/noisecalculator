namespace NoiseCalculator.Domain.Entities
{
    public class HelicopterNoiseDosage
    {
        public virtual int Id { get; private set; }
        public virtual HelicopterType HelicopterType { get; set; }
        public virtual HelicopterNoiseProtection HelicopterNoiseProtection { get; set; }
        public virtual HelicopterWorkInterval HelicopterWorkInterval { get; set; }
        public virtual int NoiseLevel { get; set; }
    }
}
