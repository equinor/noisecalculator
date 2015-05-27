namespace NoiseCalculator.Domain.Entities
{
    public class NoiseProtection
    {
        public virtual int Id { get; set; }
        public virtual NoiseProtectionDefinition NoiseProtectionDefinition { get; set; }
        public virtual string Title { get; set; }
        public virtual string CultureName { get; set; }
        public virtual int NoiseDampening { get; set; }
    }
}
