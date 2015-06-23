using System.Collections.Generic;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.DataAccess.Interfaces
{
    public interface ITaskDAO : IDAO<Task, int>
    {
        IEnumerable<Task> GetAllOrdered();
        IEnumerable<Task> GetAllByTaskDefinitionIdOrdered(int id);
        IEnumerable<Task> GetAllHelideckByTaskDefinitionIdOrdered(int id);
        //
    }
}
