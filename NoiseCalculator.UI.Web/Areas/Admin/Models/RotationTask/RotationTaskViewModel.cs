using System.Collections.Generic;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.Models.RotationTask
{
    public class RotationTaskViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        //public int TaskId { get; set; }
        public int TaskDefinitionId { get; set; }
        public IList<SelectOptionViewModel> OperatorTasks { get; set; }
        public IList<SelectOptionViewModel> AssistantTasks { get; set; }

        public RotationTaskViewModel()
        {
            OperatorTasks = new List<SelectOptionViewModel>();
            AssistantTasks = new List<SelectOptionViewModel>();
        }
    }
}