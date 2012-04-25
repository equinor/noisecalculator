namespace NoiseCalculator.Domain.Entities
{
    public class HelicopterNoiseProtection
    {
        public virtual int Id { get; set; }
        public virtual HelicopterNoiseProtectionDefinition HelicopterNoiseProtectionDefinition { get; set; }
        public virtual string Title { get; set; }
        public virtual string CultureName { get; set; }
    }
}
