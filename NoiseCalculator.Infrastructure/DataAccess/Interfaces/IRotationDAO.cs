using System.Collections.Generic;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.DataAccess.Interfaces
{
    public interface IRotationDAO : IDAO<Rotation,int>
    {
        Rotation GetByTaskId(int taskId);
        IEnumerable<Rotation> GetAllFetchTaskOrderedByTaskTitle();
    }
}
