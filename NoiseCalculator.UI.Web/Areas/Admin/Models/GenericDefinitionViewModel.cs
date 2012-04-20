using System.Collections.Generic;

namespace NoiseCalculator.UI.Web.Areas.Admin.Models
{
    public class GenericDefinitionViewModel
    {
        public int Id { get; set; }
        public string SystemName { get; set; }
        public IList<GenericTranslationViewModel> Translations { get; set; }

        public string UrlCreateTranslation { get; set; }


        public GenericDefinitionViewModel()
        {
            SystemName = string.Empty;
            UrlCreateTranslation = string.Empty;
            Translations = new List<GenericTranslationViewModel>();
        }
    }
}