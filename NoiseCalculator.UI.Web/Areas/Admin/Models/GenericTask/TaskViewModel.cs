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
        public string SelectedLanguage { get; private set; }
        public string SelectedCultureName { get; private set; }
        public int DefinitionId { get; set; }

        public TaskViewModel(string selectedCultureName)
        {
            SelectedCultureName = selectedCultureName;
            SelectedLanguage = LanguageResolver.GetLanguageName(selectedCultureName);
            Languages = new List<SelectOptionViewModel>();
            Roles = new List<SelectOptionViewModel>();
            NoiseProtections = new List<SelectOptionViewModel>();

            AddLanguageSelectOption("-- Select One --", string.Empty);
            AddLanguageSelectOption(LanguageResolver.GetLanguageName("nb-NO"), "nb-NO");
            AddLanguageSelectOption(LanguageResolver.GetLanguageName("en-US"), "en-US");
        }

        private void AddLanguageSelectOption(string text, string value)
        {
            SelectOptionViewModel selectOptionViewModel = new SelectOptionViewModel(text, value) { IsSelected = (value == SelectedCultureName) };
            Languages.Add(selectOptionViewModel);
        }
    }
}