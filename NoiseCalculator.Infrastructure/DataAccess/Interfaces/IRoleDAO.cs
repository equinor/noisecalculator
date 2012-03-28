using System.Collections.Generic;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.DataAccess.Interfaces
{
    public interface IRoleDAO : IDAO<Role, int>
    {
        Role Get(string title);
        IEnumerable<int> GetAreaNoiseRoleIds();
    }
}
