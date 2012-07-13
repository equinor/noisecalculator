using System.Collections.Generic;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.Models.RotationTask
{
    public class RotationTaskViewModel
    {
        public int Id { get; set; }
        public IList<SelectOptionViewModel> Languages { get; private set; }
        public string SelectedCultureName { get; private set; }
        public string Title { get; set; }
        public int TaskDefinitionId { get; set; }
        public IList<SelectOptionViewModel> OperatorTasks { get; set; }
        public IList<SelectOptionViewModel> AssistantTasks { get; set; }

        public RotationTaskViewModel(IList<SelectOptionViewModel> languages)
        {
            Languages = languages;
            OperatorTasks = new List<SelectOptionViewModel>();
            AssistantTasks = new List<SelectOptionViewModel>();
        }
    }
}