using System.Collections.Generic;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.DataAccess.Interfaces
{
    public interface ITaskDefinitionDAO : IDAO<TaskDefinition, int>
    {
        IEnumerable<TaskDefinition> GetAllOrdered();
    }
}
