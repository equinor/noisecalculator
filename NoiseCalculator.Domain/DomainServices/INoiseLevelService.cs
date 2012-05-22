using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Domain.Enums;

namespace NoiseCalculator.Domain.DomainServices
{
    public interface INoiseLevelService
    {
        NoiseLevelEnum CalculateNoiseLevelEnum(int percentageDailyDosage);
        string GetNoiseLevelStatusText(NoiseLevelEnum noiseLevelEnum);
    }
}