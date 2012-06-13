using System.Collections.Generic;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Domain.Enums;

namespace NoiseCalculator.Infrastructure.DataAccess.Interfaces
{
    public interface IRoleDAO : IDAO<Role, int>
    {
        Role Get(string systemTitle, string cultureName);
        IEnumerable<int> GetAreaNoiseRoleIds();
    }
}
