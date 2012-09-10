using System.Collections.Generic;

namespace NoiseCalculator.UI.Web.Areas.Admin.ViewModels.TaskDefinition
{
    public class TaskDefinitionIndexViewModel
    {
        public IList<TaskDefinitionListItemViewModel> Definitions { get; private set; }

        public TaskDefinitionIndexViewModel()
        {
            Definitions = new List<TaskDefinitionListItemViewModel>();
        }
    }
}