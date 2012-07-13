using System.Collections.Generic;
using NoiseCalculator.UI.Web.Models;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.Models.Generic
{
    public class GenericTranslationViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IList<SelectOptionViewModel> Languages { get; private set; }
        public string FormActionUrl { get; set; }
        public string SelectedLanguage { get; private set; }
        public string SelectedCultureName { get; private set; }
        public int DefinitionId { get; set; }

        public GenericTranslationViewModel(string selectedCultureName)
        {
            SelectedCultureName = selectedCultureName;
            SelectedLanguage = LanguageResolver.GetLanguageName(selectedCultureName);
            Languages = new List<SelectOptionViewModel>();

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