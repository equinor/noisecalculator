using System.Collections.Generic;
using System.Web.Mvc;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.UI.Web.ViewModels
{
    public class TaskSelectViewModel
    {
        public IList<SelectListItem> Tasks { get; set; }
        public IList<SelectListItem> TaskDefinitions { get; set; }
        public int TaskId { get; set; }
        public int TaskDefinitionId { get; set; }
        
        public TaskSelectViewModel(IList<SelectListItem> tasks, IList<SelectListItem> taskDefinitions)
        {
            Tasks = tasks;
            TaskDefinitions = taskDefinitions;
        }

        public TaskSelectViewModel()
        {
            Tasks = new List<SelectListItem>();
            TaskDefinitions = new List<SelectListItem>();
        }
    }

}