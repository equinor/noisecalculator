using System.Collections.Generic;
using NoiseCalculator.UI.Web.Support;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.Models
{
    public class LanguageListBuilder
    {
        public IList<SelectOptionViewModel> CreateSelectedLanguageList(string selectedCultureName)
        {
            IList<SelectOptionViewModel> languages = new List<SelectOptionViewModel>();

            languages.Add(CreateLanguageSelectOption("-- Select One --", "0", selectedCultureName));
            languages.Add(CreateLanguageSelectOption(LanguageResolver.GetLanguageName("nb-NO"), "nb-NO", selectedCultureName));
            languages.Add(CreateLanguageSelectOption(LanguageResolver.GetLanguageName("en-US"), "en-US", selectedCultureName));

            return languages;
        }

        private SelectOptionViewModel CreateLanguageSelectOption(string text, string value, string selectedCultureName)
        {
            SelectOptionViewModel selectOption = new SelectOptionViewModel(text, value);
            if(string.IsNullOrEmpty(selectedCultureName) == false)
            {
                selectOption.IsSelected = (value == selectedCultureName);
            }
            
            return selectOption;
        }
    }
}