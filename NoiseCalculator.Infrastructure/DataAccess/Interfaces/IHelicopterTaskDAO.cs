﻿using System.Collections.Generic;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.DataAccess.Interfaces
{
    public interface IHelicopterTaskDAO : IDAO<HelicopterTask, int>
    {
        HelicopterTask Get(int helicopterId, NoiseProtectionDefinition noiseProtectionDefinition);
        HelicopterTask Get(int helicopterId, int noiseProtectionDefinitionId);
        IEnumerable<HelicopterTask> GetAllOrderedByType();
    }
}
