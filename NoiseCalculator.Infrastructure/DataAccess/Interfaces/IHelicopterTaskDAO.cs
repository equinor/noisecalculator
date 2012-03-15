﻿using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.DataAccess.Interfaces
{
    public interface IHelicopterTaskDAO : IDAO<HelicopterTask, int>
    {
        HelicopterTask Get(int helicopterId, HelicopterNoiseProtectionDefinition noiseProtectionDefinition, int workIntervalId);
    }
}
