using System;

namespace NoiseCalculator.Domain.Entities
{
    public class Task
    {
        public virtual int Id { get; private set; }
        public virtual string Title { get; set; }
        public virtual Role Role { get; set; }
        public virtual NoiseProtection NoiseProtection { get; set; }
        public virtual int NoiseLevelGuideline { get; set; }
        public virtual int AllowedExposureMinutes { get; set; }

        
        public virtual decimal CalculatePercentage(int actualNoiseLevel, TimeSpan actualExposure )
        {
            decimal adjustmentFactorForMeassuredNoise = CalculateAdjustmentFactorForMeassuredNoise(actualNoiseLevel);
            decimal calculatedPercentage = ((int)actualExposure.TotalMinutes * 100) / (AllowedExposureMinutes * adjustmentFactorForMeassuredNoise);
            
            return calculatedPercentage;
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
