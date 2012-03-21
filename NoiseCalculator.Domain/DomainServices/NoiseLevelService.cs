using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Domain.DomainServices
{
    public class NoiseLevelService : INoiseLevelService
    {
        public NoiseLevelEnum CalculateNoiseLevelEnum(int percentageDailyDosage)
        {
            if (percentageDailyDosage > 100)
            {
                return NoiseLevelEnum.Critical;
            }
            if (percentageDailyDosage >= 75 && percentageDailyDosage <= 100)
            {
                return NoiseLevelEnum.Warning;
            }

            return NoiseLevelEnum.Normal;
        }
    }
}
