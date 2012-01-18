using System;
using System.Collections.Generic;

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
        public virtual HelicopterTask HelicopterTask { get; set; }

        // Non persisted property
        // public virtual int NoiseLevelMeasured { get; set; }
        //public virtual int ActualExposure { get; set; }


        //public virtual decimal CalculateDailyDosagePercentage(int noiseLevelMeasured)
        //public virtual decimal CalculateDailyDosagePercentage(int actualNoiseLevel)
        public virtual decimal CalculateDailyDosagePercentage(int actualNoiseLevel, TimeSpan actualExposure )
        {
            decimal adjustmentFactorForMeassuredNoise = 1;

            //if(NoiseLevelMeasured > NoiseLevelGuideline)
            if (actualNoiseLevel > NoiseLevelGuideline)
            {
                //int difference = (NoiseLevelMeasured - NoiseLevelGuideline);
                int difference = (actualNoiseLevel - NoiseLevelGuideline);

                switch (difference)
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

            //decimal calculatedPercentage = (ActualExposure * 100) / (AllowedExposureMinutes * adjustmentFactorForMeassuredNoise);
            decimal calculatedPercentage = (actualExposure.Minutes * 100) / (AllowedExposureMinutes * adjustmentFactorForMeassuredNoise);
            
            return calculatedPercentage;
        }
    }
}
