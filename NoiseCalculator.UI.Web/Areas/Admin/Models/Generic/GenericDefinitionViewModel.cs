using System.Collections.Generic;

namespace NoiseCalculator.UI.Web.Areas.Admin.Models.Generic
{
    public class GenericDefinitionViewModel
    {
        public int Id { get; set; }
        public string SystemName { get; set; }
        
        public bool HasTranslationSupport { get; set; }
        public IList<GenericTranslationViewModel> Translations { get; set; }

        public string UrlCreateTranslation { get; set; }
        public string UrlEditTranslation { get; set; }
        public string UrlDeleteTranslationConfirmation { get; set; }


        public GenericDefinitionViewModel()
        {
            HasTranslationSupport = true;
            SystemName = string.Empty;
            UrlCreateTranslation = string.Empty;
            UrlEditTranslation = string.Empty;
            UrlDeleteTranslationConfirmation = string.Empty;
            Translations = new List<GenericTranslationViewModel>();
        }
    }
}