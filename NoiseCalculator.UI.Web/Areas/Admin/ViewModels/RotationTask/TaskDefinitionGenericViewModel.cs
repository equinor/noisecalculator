using System.Collections.Generic;

namespace NoiseCalculator.UI.Web.Areas.Admin.ViewModels.RotationTask
{
    public class TaskDefinitionRotationViewModel
    {
        public int Id { get; set; }
        public string SystemName { get; set; }
        public string RoleType { get; set; }

        public IList<RotationTaskListItemViewModel> RotationTasks { get; set; }

        public TaskDefinitionRotationViewModel()
        {
            RotationTasks = new List<RotationTaskListItemViewModel>();
        }
    }
}