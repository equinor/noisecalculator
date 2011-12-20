using System.Collections.Generic;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Domain.DomainServices
{
    public interface ITaskCalculator
    {
        int CalculateTotalDailyNoiseDosage(IEnumerable<Task> tasks);
    }
}