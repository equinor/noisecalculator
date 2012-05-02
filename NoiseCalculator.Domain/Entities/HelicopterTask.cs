namespace NoiseCalculator.Domain.Entities
{
    public class HelicopterTask
    {
        public virtual int Id { get; set; }
        public virtual HelicopterType HelicopterType { get; set; }
        
        // Commented while testing new data model
        //public virtual HelicopterNoiseProtectionDefinition HelicopterNoiseProtectionDefinition { get; set; }
        public HelicopterNoiseProtection HelicopterNoiseProtection { get; set; } // <--- For testing
        
        public virtual HelicopterWorkInterval HelicopterWorkInterval { get; set; }
        public virtual int Percentage { get; set; }

        // New, for admin and translation
        public string CultureName { get; set; }
        public HelicopterTaskDefinition HelicopterTaskDefinition { get; set; }

        public virtual int GetMaximumAllowedMinutes()
        {
            char[] splitters = new char[] { ' ', '-' };
            string[] minuteElements = HelicopterWorkInterval.Title.Split(splitters);
            int minutes = int.Parse(minuteElements[1]);
            
            return minutes;
        }
    }
}
