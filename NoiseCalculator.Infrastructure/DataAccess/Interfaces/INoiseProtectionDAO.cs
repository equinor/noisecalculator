using System.Collections.Generic;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.DataAccess.Interfaces
{
    public interface INoiseProtectionDAO : IDAO<NoiseProtection, int>
    {
        IEnumerable<NoiseProtection> GetAllByCultureName(string cultureName);
    }
}
