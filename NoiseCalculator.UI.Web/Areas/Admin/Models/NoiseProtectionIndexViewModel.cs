using System.Collections.Generic;
using System.Globalization;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.UI.Web.Models;
using NoiseCalculator.UI.Web.Resources;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.Models
{
    public class NoiseProtectionIndexViewModel
    {
        public IList<SelectOptionViewModel> Languages { get; private set; }
        public string SelectedLanguage { get; private set; }
        public IList<NoiseProtectionViewModel> NoiseProtections { get; private set; }

        public NoiseProtectionIndexViewModel(IEnumerable<NoiseProtection> noiseProtections) : this(noiseProtections, string.Empty)
        {
            //
        }

        public NoiseProtectionIndexViewModel(IEnumerable<NoiseProtection> noiseProtections, string selectedLanguage)
        {
            SelectedLanguage = selectedLanguage;
            
            Languages = new List<SelectOptionViewModel>();
            AddLanguageSelectOption(TaskResources.SelectOne, string.Empty);
            AddLanguageSelectOption(LanguageResolver.GetLanguageName("nb-NO"), "nb-NO");
            AddLanguageSelectOption(LanguageResolver.GetLanguageName("en-US"), "en-US");

            NoiseProtections = new List<NoiseProtectionViewModel>();
            foreach (NoiseProtection noiseProtection in noiseProtections)
            {
                NoiseProtections.Add(new NoiseProtectionViewModel{Id = noiseProtection.Id, Title = noiseProtection.Title, Language = LanguageResolver.GetLanguageName(noiseProtection.CultureName)});
            }
        }

        private void AddLanguageSelectOption(string text, string value)
        {
            SelectOptionViewModel selectOptionViewModel = new SelectOptionViewModel(text, value) { IsSelected = (value == SelectedLanguage) };
            Languages.Add(selectOptionViewModel);
        }
    }

    public class NoiseProtectionViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
    }
}