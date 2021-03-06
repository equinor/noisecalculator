using System.Collections.Generic;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.ViewModels.GenericTask
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IList<SelectOptionViewModel> Roles { get; set; }
        public IList<SelectOptionViewModel> NoiseProtections { get; set; }
        public string NoiseLevelGuideline { get; set; }
        public string Hours { get; set; }
        public string Minutes { get; set; }

        public IList<SelectOptionViewModel> Languages { get; private set; }
        public string SelectedCultureName { get; private set; }
        public int DefinitionId { get; set; }
        public int ButtonPressed { get; set; }
        public int BackgroundNoise { get; set; }
        public string Frequency { get; set; }

        public TaskViewModel(IList<SelectOptionViewModel> languages)
        {
            Languages = languages;
            Roles = new List<SelectOptionViewModel>();
            NoiseProtections = new List<SelectOptionViewModel>();
            NoiseLevelGuideline = string.Empty;
            Hours = string.Empty;
            Minutes = string.Empty;
        }
    }
}