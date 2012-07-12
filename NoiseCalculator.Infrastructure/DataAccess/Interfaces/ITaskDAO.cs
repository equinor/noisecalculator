using System.Collections.Generic;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.DataAccess.Interfaces
{
    public interface ITaskDAO : IDAO<Task, int>
    {
        IEnumerable<Task> GetAllOrdered();
        //
    }
}
