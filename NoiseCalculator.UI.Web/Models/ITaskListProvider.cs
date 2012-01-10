using System.Collections.Generic;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.UI.Web.Models
{
    public interface ITaskListProvider
    {
        IList<Task> GetTaskList();
    }
}