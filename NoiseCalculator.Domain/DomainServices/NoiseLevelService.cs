using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Domain.Resources;

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

        public string GetNoiseLevelStatusText(NoiseLevelEnum noiseLevelEnum)
        {
            switch (noiseLevelEnum)
            {
                case NoiseLevelEnum.Normal:
                    return DomainResources.NoiseLevelStatusTextNormal;
                    break;
                case NoiseLevelEnum.Warning:
                    return DomainResources.NoiseLevelStatusTextWarning;
                    break;
                case NoiseLevelEnum.Critical:
                    return DomainResources.NoiseLevelStatusTextCritical;
                    break;
            }

            return string.Empty;
        }
    }
}
