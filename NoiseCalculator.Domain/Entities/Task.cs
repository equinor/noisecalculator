using System;

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

        public virtual decimal CalculatePercentage(decimal actualNoiseLevel, int buttonPressed, int backgroundNoise, NoiseProtection noiseProtection, TimeSpan actualExposure )
        {
            var noiseProtectionDampening = noiseProtection.NoiseDampening;
            const double timeInFullShift = 720;
            if (backgroundNoise == 0)
                backgroundNoise = 80;

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

        public virtual TimeSpan CalculateTimeSpan(decimal actualNoiseLevel, int buttonPressed, int backgroundNoise, NoiseProtection noiseProtection, int percentage)
        {
            var noiseProtectionDampening = noiseProtection.NoiseDampening;
            const double timeInFullShift = 720;
            if (backgroundNoise == 0)
                backgroundNoise = 80;
            
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
                                     ((Math.Pow(10, (80/10)))/Math.Pow(10, normValueWithNoiseProtection/10));

            var timeSpan = new TimeSpan(0, 0, (int)Math.Round(allowedExposure), 0);

            if (timeSpan.Days > 0)
            {
                timeSpan = new TimeSpan(0,0,1439,0);
            }

            return timeSpan;
        }

    }
}
