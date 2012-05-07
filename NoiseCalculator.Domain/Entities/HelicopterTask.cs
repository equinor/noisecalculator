
namespace NoiseCalculator.Domain.Entities
{
    public class HelicopterTask
    {
        public virtual int Id { get; set; }
        public virtual HelicopterType HelicopterType { get; set; }
        public virtual HelicopterNoiseProtectionDefinition HelicopterNoiseProtectionDefinition { get; set; }
        public virtual HelicopterWorkInterval HelicopterWorkInterval { get; set; }
        public virtual int Percentage { get; set; }

        public virtual int GetMaximumAllowedMinutes()
        {
            char[] splitters = new char[] { ' ', '-' };
            string[] minuteElements = HelicopterWorkInterval.Title.Split(splitters);
            int minutes = int.Parse(minuteElements[1]);
            
            return minutes;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} - {2}", HelicopterType.Title, HelicopterNoiseProtectionDefinition.SystemName, HelicopterWorkInterval.Title);
        }
    }
}
