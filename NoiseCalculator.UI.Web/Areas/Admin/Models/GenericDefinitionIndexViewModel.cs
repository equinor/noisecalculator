using System.Collections.Generic;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.UI.Web.Models;
using NoiseCalculator.UI.Web.Resources;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.Models
{
    public class GenericDefinitionIndexViewModel
    {
        public string PageTitle { get; set; }
        //public IList<SelectOptionViewModel> Languages { get; private set; }
        //public string SelectedLanguage { get; private set; }
        public IList<GenericDefinitionViewModel> Definitions { get; private set; }
        
        public string UrlCreate { get; set; }
        public string UrlEdit { get; set; }
        public string UrlDeleteConfirmation { get; set; }
        //public string UrlDeleteDefinition { get; set; }

        //public GenericDefinitionIndexViewModel(IEnumerable<NoiseProtectionDefinition> noiseProtectionsDefinitions) : this(noiseProtectionsDefinitions, string.Empty)
        //{
        //    //
        //}

        //public GenericDefinitionIndexViewModel(IEnumerable<NoiseProtectionDefinition> noiseProtectionsDefinitions, string selectedLanguage)
        //public GenericDefinitionIndexViewModel(IEnumerable<NoiseProtectionDefinition> noiseProtectionsDefinitions)
        public GenericDefinitionIndexViewModel()
        {
            //SelectedLanguage = selectedLanguage;
            
            //Languages = new List<SelectOptionViewModel>();
            //AddLanguageSelectOption(TaskResources.SelectOne, string.Empty);
            //AddLanguageSelectOption(LanguageResolver.GetLanguageName("nb-NO"), "nb-NO");
            //AddLanguageSelectOption(LanguageResolver.GetLanguageName("en-US"), "en-US");

            Definitions = new List<GenericDefinitionViewModel>();
            //foreach (NoiseProtectionDefinition noiseProtDefinitions in noiseProtectionsDefinitions)
            //{
            //    Definitions.Add(new GenericDefinitionViewModel { Id = noiseProtDefinitions.Id, Title = noiseProtDefinitions.Title});
            //}

            //NoiseProtections = new List<NoiseProtectionViewModel>();
            //foreach (NoiseProtection noiseProtection in noiseProtectionsDefinitions)
            //{
            //    NoiseProtections.Add(new NoiseProtectionViewModel{Id = noiseProtection.Id, Title = noiseProtection.Title, Language = LanguageResolver.GetLanguageName(noiseProtection.CultureName)});
            //}
        }

        //private void AddLanguageSelectOption(string text, stringvalue)
        //{
        //    SelectOptionViewModel selectOptionViewModel = new SelectOptionViewModel(text, value) { IsSelected = (value == SelectedLanguage) };
        //    Languages.Add(selectOptionViewModel);
        //}
    }
}