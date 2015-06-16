using System.Collections.Generic;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.ViewModels.HelicopterTask
{
    public class HelicopterTaskViewModel
    {
        public int Id { get; set; }
        public IList<SelectOptionViewModel> Helicopters { get; set; }
        public IList<SelectOptionViewModel> Tasks { get; set; }
        public decimal NoiseLevel { get; set; }
        
        public string UrlCreateTranslation { get; set; }
        public string UrlEditTranslation { get; set; }
        public string UrlDeleteTranslationConfirmation { get; set; }


        public HelicopterTaskViewModel()
        {
            Helicopters = new List<SelectOptionViewModel>();
            Tasks = new List<SelectOptionViewModel>();
            NoiseLevel = 0;

            UrlCreateTranslation = string.Empty;
            UrlEditTranslation = string.Empty;
            UrlDeleteTranslationConfirmation = string.Empty;
        }
    }
}