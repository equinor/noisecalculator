using System.Collections.Generic;
using NoiseCalculator.UI.Web.Models;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.Models.GenericTask
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IList<SelectOptionViewModel> Roles { get; set; }
        public IList<SelectOptionViewModel> NoiseProtections { get; set; }
        public int NoiseLevelGuideline { get; set; }
        public int AllowedExposureMinutes { get; set; }
        public IList<SelectOptionViewModel> Languages { get; private set; }
        public string SelectedCultureName { get; private set; }
        public int DefinitionId { get; set; }

        public TaskViewModel(IList<SelectOptionViewModel> languages)
        {
            Languages = languages;
            Roles = new List<SelectOptionViewModel>();
            NoiseProtections = new List<SelectOptionViewModel>();
        }
    }
}