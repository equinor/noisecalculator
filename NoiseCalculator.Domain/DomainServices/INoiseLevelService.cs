using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Domain.DomainServices
{
    public interface INoiseLevelService
    {
        NoiseLevelEnum CalculateNoiseLevelEnum(int percentageDailyDosage);
        string GetNoiseLevelStatusText(NoiseLevelEnum noiseLevelEnum);
    }
}