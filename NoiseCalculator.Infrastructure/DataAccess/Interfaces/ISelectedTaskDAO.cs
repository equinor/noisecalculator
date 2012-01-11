using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.DataAccess.Interfaces
{
    public interface ISelectedTaskDAO : IDAO<SelectedTask,int>
    {
        IEnumerable<SelectedTask> GetAllChronologically(string createdByUsername, DateTime createdDate);
    }
}
