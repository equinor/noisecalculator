﻿using System.Collections.Generic;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.DataAccess.Interfaces
{
    public interface IHelicopterTaskDAO : IDAO<HelicopterTask, int>
    {
        HelicopterTask Get(int helicopterId, HelicopterNoiseProtectionDefinition noiseProtectionDefinition, int workIntervalId);
        HelicopterTask Get(int helicopterId, int noiseProtectionDefinitionId, int workIntervalId);
        IEnumerable<HelicopterTask> GetAllOrderedByTypeAndWorkInterval();
    }
}
