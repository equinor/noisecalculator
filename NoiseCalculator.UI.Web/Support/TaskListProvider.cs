using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.UI.Web.Models
{
    public class TaskListProvider : ITaskListProvider
    {
        public IList<Task> GetTaskList()
        {
            return null;
        }
    }
}