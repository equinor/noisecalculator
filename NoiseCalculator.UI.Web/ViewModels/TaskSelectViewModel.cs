using System.Collections.Generic;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.UI.Web.ViewModels
{
    public class TaskSelectViewModel
    {
        public IEnumerable<Task> Tasks { get; set; }
        public IEnumerable<TaskDefinition> TaskDefinitions { get; set; }
        
        public TaskSelectViewModel(IEnumerable<Task> tasks, IEnumerable<TaskDefinition> taskDefinitions)
        {
            Tasks = tasks;
            TaskDefinitions = taskDefinitions;
        }

    }
}