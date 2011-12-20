namespace NoiseCalculator.Domain.Entities
{
    public class Task
    {
        public virtual int Id { get; private set; }
        public virtual string Title { get; set; }
        public virtual Role Role { get; set; }
        public virtual NoiseProtection NoiseProtection { get; set; }
        public virtual int NoiseLevelGuideline { get; set; }
        public virtual int AllowedExposure { get; set; }

        // Non persisted property
        public virtual int NoiseLevelMeasured { get; set; }
        public virtual int ActualExposure { get; set; }


        public decimal CalculateDailyDosagePercentage()
        {
            decimal adjustmentFactorForMeassuredNoise = 1;

            if(NoiseLevelMeasured > NoiseLevelGuideline)
            {
                int difference = (NoiseLevelMeasured - NoiseLevelGuideline);

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

            decimal calculatedPercentage = (ActualExposure * 100) / (AllowedExposure * adjustmentFactorForMeassuredNoise);
            
            return calculatedPercentage;
        }
    }
}
