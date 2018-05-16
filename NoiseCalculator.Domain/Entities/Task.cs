using System;
using NoiseCalculator.Domain.Enums;

namespace NoiseCalculator.Domain.Entities
{
    public class Task
    {
        public virtual int Id { get; set; }
        public virtual TaskDefinition TaskDefinition { get; set; }
        public virtual string Title { get; set; }
        public virtual Role Role { get; set; }
        public virtual NoiseProtection NoiseProtection { get; set; }
        public virtual decimal NoiseLevelGuideline { get; set; }
        public virtual int AllowedExposureMinutes { get; set; }
        public virtual string CultureName { get; set; }
        public virtual int SortOrder { get; set; }
        public virtual int ButtonPressed { get; set; }
        public virtual int NoiseProtectionId { get; set; }
        public virtual string Frequency { get; set; }
        public virtual bool UsePercentage { get; set; }

        public virtual decimal CalculatePercentage(string roleType, decimal actualNoiseLevel, int buttonPressed, int backgroundNoise, string frequency, NoiseProtection noiseProtection, TimeSpan actualExposure )
        {   
            var noiseProtectionDampening = noiseProtection.NoiseDampening;

            if (frequency == "M" && noiseProtection.NoiseProtectionDefinition.Id == 1)
                noiseProtectionDampening = 16;
            if (frequency == "H" && noiseProtection.NoiseProtectionDefinition.Id == 1)
                noiseProtectionDampening = 18;
            if (frequency == "M" && noiseProtection.NoiseProtectionDefinition.Id == 2)
                noiseProtectionDampening = 22;
            if (frequency == "H" && noiseProtection.NoiseProtectionDefinition.Id == 2)
                noiseProtectionDampening = 24;

            

            // Special handling for areanoise
            if (roleType == RoleTypeEnum.AreaNoise.ToString() && noiseProtection.NoiseProtectionDefinition.Id == 3)
            {
                if (actualNoiseLevel > 110)
                    return (decimal)(actualExposure.TotalMinutes * 0.441667);
                if (actualNoiseLevel > 105)
                    return (decimal)(actualExposure.TotalMinutes * 0.138888);
                if (actualNoiseLevel > 100)
                    return (decimal)(actualExposure.TotalMinutes * 0.043750);
                if (actualNoiseLevel > 95)
                    return (decimal)(actualExposure.TotalMinutes * 0.013541);
                if (actualNoiseLevel > 90)
                    return (decimal)(actualExposure.TotalMinutes * 0.004167);
                if (actualNoiseLevel > 85)
                    return (decimal)(actualExposure.TotalMinutes * 0.001042);
                return (decimal)(0);
            }
            // Special handling for areanoise
            if (roleType == RoleTypeEnum.AreaNoise.ToString())
            {
                if (actualNoiseLevel > 110)
                    return 101;
                if (actualNoiseLevel > 105)
                    return (decimal)(actualExposure.TotalMinutes * 3.333333);
                if (actualNoiseLevel > 100)
                    return (decimal)(actualExposure.TotalMinutes * 0.833333);
                if (actualNoiseLevel > 95)
                    return (decimal)(actualExposure.TotalMinutes * 0.277777);
                if (actualNoiseLevel > 90)
                    return (decimal)(actualExposure.TotalMinutes * 0.277777);
                if (actualNoiseLevel > 85)
                    return (decimal)(actualExposure.TotalMinutes * 0.138888);
                return (decimal)(actualExposure.TotalMinutes * 0.027712);
            }

            const double timeInFullShift = 720;
            if (backgroundNoise == 0)
                backgroundNoise = 70;

            // Støynivå => 10* LOG(10^(støydef/10) + 10^(bakgrunnsstøy/10)
            var noiseLevel = 10*
                             Math.Log((Math.Pow(10, ((double)actualNoiseLevel / 10)) +
                                      Math.Pow(10, ((double)backgroundNoise/10))), 10.0);

            // Norm verdi => 10 * LOG (10^(støynivå/10)) * knappen inne / 100)
            var normalizedValue = 10 * Math.Log((Math.Pow(10, (noiseLevel/10)) * ((double)buttonPressed / 100)) + Math.Pow(10, ((double)backgroundNoise / 10)) * (((100 - (double)buttonPressed)/100)), 10.0);

            // Norm verdi med hørselsvern
            var normValueWithNoiseProtection = normalizedValue - noiseProtectionDampening;

            var percentMinutes = actualExposure.TotalMinutes/timeInFullShift;

            // Eksponering i db => 10 * LOG (Time in minutes / Time in full shift * 10 ^ (Noise - noiseprotection / 10)
            var exposure = 10 *
                           Math.Log((percentMinutes *
                                    Math.Pow(10, (normValueWithNoiseProtection / 10))), 10.0);

            // % beregning => 10^(exposure/10))/(10^(80/10))*100)
            var calcPerc = (Math.Pow(10, (exposure / 10))) / (Math.Pow(10, (80 / 10))) * 100;
            
            return (decimal)calcPerc;
        }

        public virtual TimeSpan CalculateTimeSpan(string roleType, decimal actualNoiseLevel, int buttonPressed, int backgroundNoise, string frequency, NoiseProtection noiseProtection, int percentage)
        {
            var noiseProtectionDampening = noiseProtection.NoiseDampening;

            if (frequency == "M" && noiseProtection.NoiseProtectionDefinition.Id == 1)
                noiseProtectionDampening = 16;
            if (frequency == "H" && noiseProtection.NoiseProtectionDefinition.Id == 1)
                noiseProtectionDampening = 18;
            if (frequency == "M" && noiseProtection.NoiseProtectionDefinition.Id == 2)
                noiseProtectionDampening = 22;
            if (frequency == "H" && noiseProtection.NoiseProtectionDefinition.Id == 2)
                noiseProtectionDampening = 24;

            // Special handling for areanoise
            if (roleType == RoleTypeEnum.AreaNoise.ToString() && noiseProtection.NoiseProtectionDefinition.Id == 3)
            {
                if (actualNoiseLevel > 110)
                    return TimeSpan.FromMinutes(percentage / 0.438596);
                if (actualNoiseLevel > 105)
                    return TimeSpan.FromMinutes(percentage / 0.138888);
                if (actualNoiseLevel > 100)
                    return TimeSpan.FromMinutes(percentage / 0.043750);
                if (actualNoiseLevel > 95)
                    return TimeSpan.FromMinutes(percentage / 0.013541);
                if (actualNoiseLevel > 90)
                    return TimeSpan.FromMinutes(percentage / 0.004167);
                if (actualNoiseLevel > 85)
                    return TimeSpan.FromMinutes(percentage / 0.001042);
                return new TimeSpan(0, 0, 960, 0);
            }
            // Special handling for areanoise
            if (roleType == RoleTypeEnum.AreaNoise.ToString())
            {
                if (actualNoiseLevel > 110)
                    return new TimeSpan();
                if (actualNoiseLevel > 105)
                    return TimeSpan.FromMinutes(percentage / 3.333333);
                if (actualNoiseLevel > 100)
                    return TimeSpan.FromMinutes(percentage / 0.833333);
                if (actualNoiseLevel > 95)
                    return TimeSpan.FromMinutes(percentage / 0.277777);
                if (actualNoiseLevel > 90)
                    return TimeSpan.FromMinutes(percentage / 0.277777);
                if (actualNoiseLevel > 85) 
                    return TimeSpan.FromMinutes(percentage/0.138888);
                return TimeSpan.FromMinutes(percentage / 0.104166).Days > 0 ? new TimeSpan(0, 16, 0, 0) : TimeSpan.FromMinutes(percentage/0.104166);
            }

            const double timeInFullShift = 720;
            if (backgroundNoise == 0)
                backgroundNoise = 70;
            
            // Støynivå => 10* LOG(10^(støydef/10) + 10^(bakgrunnsstøy/10)
            var noiseLevel = 10 *
                             Math.Log((Math.Pow(10, ((double)actualNoiseLevel / 10)) +
                                      Math.Pow(10, ((double)backgroundNoise / 10))), 10.0);

            // Norm verdi => 10 * LOG (10^(støynivå/10)) * knappen inne / 100) +  10 ^(bakgrunnsstøy/10) * (100 - knappen inne)/100);
            var normalizedValue = 10 * Math.Log((Math.Pow(10, (noiseLevel / 10)) * ((double)buttonPressed / 100)) + Math.Pow(10, ((double)backgroundNoise / 10)) * (((100 - (double)buttonPressed)/100)), 10.0);

            // Norm verdi med hørselsvern
            var normValueWithNoiseProtection = normalizedValue - noiseProtectionDampening;

            // Prosent * tid full skift * (10^(80/10))/(10^(Normalisert verdi med hørselsvern/10))
            var allowedExposure = ((double)percentage / 100) * timeInFullShift *
                                        ((Math.Pow(10, (80 / 10))) / Math.Pow(10, normValueWithNoiseProtection / 10));

            var timeSpan = new TimeSpan(0, 0, (int)Math.Round(allowedExposure), 0);

            if (timeSpan.Days > 0 || timeSpan.Days == 0 && timeSpan.Hours >= 16)
            {
                timeSpan = new TimeSpan(0,16,0,0);
            }

            return timeSpan;
        }
        
    }
}
