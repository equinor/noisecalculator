using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.DataAccess.Interfaces
{
    public interface IHelicopterTaskDAO : IDAO<HelicopterTask, int>
    {
        //HelicopterTask Get(int helicopterTypeId, int helicopterNoiseProtectionId, int helicopterWorkIntervalId);
        //HelicopterTask Get(int helicopterId, int noiseProtectionDefinitionId, int workIntervalId);
        HelicopterTask Get(int helicopterId, HelicopterNoiseProtectionDefinition noiseProtectionDefinition, int workIntervalId);
    }
}
