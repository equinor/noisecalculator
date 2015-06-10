using System.Collections.Generic;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.ViewModels.HelicopterTask
{
    public class HelicopterTaskViewModel
    {
        public int Id { get; set; }
        public IList<SelectOptionViewModel> Helicopters { get; set; }
        public IList<SelectOptionViewModel> NoiseProtectionDefinitions { get; set; }
        public int ButtonPressed { get; set; }
        public int FixedTime { get; set; }
        
        public string UrlCreateTranslation { get; set; }
        public string UrlEditTranslation { get; set; }
        public string UrlDeleteTranslationConfirmation { get; set; }


        public HelicopterTaskViewModel()
        {
            Helicopters = new List<SelectOptionViewModel>();
            NoiseProtectionDefinitions = new List<SelectOptionViewModel>();
            ButtonPressed = 100;
            FixedTime = 0;

            UrlCreateTranslation = string.Empty;
            UrlEditTranslation = string.Empty;
            UrlDeleteTranslationConfirmation = string.Empty;
        }
    }
}