namespace NoiseCalculator.UI.Web.Areas.Admin.Models.HelicopterTask
{
    public class HelicopterTaskListItemViewModel
    {
        public int Id { get; set; }
        public string Helicopter { get; set; }
        public string NoiseProtectionDefinition { get; set; }
        public string WorkInterval { get; set; }
        public int Percentage { get; set; }
        //public int DefinitionId { get; set; }

        //public IList<SelectOptionViewModel> NoiseProtectionDefinitions { get; set; }


        //Languages = new List<SelectOptionViewModel>();
        
        //    AddLanguageSelectOption("-- Select One --", string.Empty);
        //    AddLanguageSelectOption(LanguageResolver.GetLanguageName("nb-NO"), "nb-NO");
        //    AddLanguageSelectOption(LanguageResolver.GetLanguageName("en-US"), "en-US");
        //}

        //private void AddLanguageSelectOption(string text, string value)
        //{
        //    SelectOptionViewModel selectOptionViewModel = new SelectOptionViewModel(text, value) { IsSelected = (value == SelectedCultureName) };
        //    Languages.Add(selectOptionViewModel);
        //}
    }
}