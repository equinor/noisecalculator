using System.Collections.Generic;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.ViewModels.HelicopterTask
{
    public class HelicopterTaskViewModel
    {
        public int Id { get; set; }
        public IList<SelectOptionViewModel> Helicopters { get; set; }
        public IList<SelectOptionViewModel> NoiseProtectionDefinitions { get; set; }
        public IList<SelectOptionViewModel> WorkIntervals { get; set; }
        public int Percentage { get; set; }
        
        public string UrlCreateTranslation { get; set; }
        public string UrlEditTranslation { get; set; }
        public string UrlDeleteTranslationConfirmation { get; set; }


        public HelicopterTaskViewModel()
        {
            Helicopters = new List<SelectOptionViewModel>();
            NoiseProtectionDefinitions = new List<SelectOptionViewModel>();
            WorkIntervals = new List<SelectOptionViewModel>();

            UrlCreateTranslation = string.Empty;
            UrlEditTranslation = string.Empty;
            UrlDeleteTranslationConfirmation = string.Empty;
        }
    }
}