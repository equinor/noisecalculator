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
        public virtual int NoiseLevelGuideline { get; set; }
        public virtual int AllowedExposureMinutes { get; set; }
        public virtual string CultureName { get; set; }
        public virtual int SortOrder { get; set; }
        public virtual int ButtonPressed { get; set; }
        public virtual int BackgroundNoise { get; set; }


        public virtual decimal CalculatePercentage(int actualNoiseLevel, int buttonPressed, int backgroundNoise, TimeSpan actualExposure )
        {
            var adjustmentFactorForMeassuredNoise = CalculateAdjustmentFactorForMeassuredNoise(actualNoiseLevel);

            var noiseProtectionDampening = 18;
            var timeInFullShift = 720;

            // Støynivå => 10* LOG(10^(støydef/10) + 10^(bakgrunnsstøy/10)
            var noiseLevel = 10*
                             Math.Log((Math.Pow(10, ((double)actualNoiseLevel / 10)) +
                                      Math.Pow(10, ((double)backgroundNoise/10))), 10.0);

            // Norm verdi => 10 * LOG (10^(støynivå/10)) * knappen inne / 100)
            var normalizedValue = 10 * Math.Log((Math.Pow(10, (noiseLevel/10))) * ((double)buttonPressed / 100), 10.0);

            // Norm verdi med hørselsvern
            var normValueWithNoiseProtection = normalizedValue - noiseProtectionDampening;

            var percentMinutes = (double)((double)actualExposure.TotalMinutes/(double)timeInFullShift);

            // Eksponering i db => 10 * LOG (Time in minutes / Time in full shift * 10 ^ (Noise - noiseprotection / 10)
            var exposure = 10 *
                           Math.Log((percentMinutes *
                                    Math.Pow(10, (normValueWithNoiseProtection / 10))), 10.0);

            // % beregning => 10^(exposure/10))/(10^(80/10))*100)
            var calcPerc = (Math.Pow(10, (exposure / 10))) / (Math.Pow(10, (80 / 10))) * 100;
            
            return (decimal)calcPerc;
        }

        public virtual TimeSpan CalculateTimeSpan(int actualNoiseLevel, int percentage)
        {
            decimal adjustmentFactorForMeassuredNoise = CalculateAdjustmentFactorForMeassuredNoise(actualNoiseLevel);
            decimal result = ( percentage * (AllowedExposureMinutes * adjustmentFactorForMeassuredNoise) ) / 100;
            TimeSpan timeSpan = new TimeSpan(0, 0, (int)Math.Round(result), 0);
            
            return timeSpan;
        }

        private decimal CalculateAdjustmentFactorForMeassuredNoise(int actualNoiseLevel)
        {
            decimal adjustmentFactorForMeassuredNoise = 1;

            if (actualNoiseLevel > NoiseLevelGuideline)
            {
                // dBA above guideline
                int noiseLevelDelta = (actualNoiseLevel - NoiseLevelGuideline);

                switch (noiseLevelDelta)
                {
                    case 3:
                        {
                            adjustmentFactorForMeassuredNoise = 0.5m;
                            break;
                        }
                    case 4:
                        {
                            adjustmentFactorForMeassuredNoise = 0.4m;
                            break;
                        }
                    case 5:
                        {
                            adjustmentFactorForMeassuredNoise = 0.3m;
                            break;
                        }
                    case 6:
                        {
                            adjustmentFactorForMeassuredNoise = 0.25m;
                            break;
                        }
                }
            }
            return adjustmentFactorForMeassuredNoise;
        }
    }
}
