using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoiseCalculator.UI.Web.Areas.Admin.Models
{
    public class HelicopterTaskViewModel
    {
        public int Id { get; set; }
        public string Helicopter { get; set; }
        public string NoiseProtection { get; set; }
        public string WorkInterval { get; set; }
        public int Percentage { get; set; }

        public bool HasTranslationSupport { get; set; }
        //public IList<GenericTranslationViewModel> Translations { get; set; }

        
        //public string UrlCreateTranslation { get; set; }
        //public string UrlEditTranslation { get; set; }
        //public string UrlDeleteTranslationConfirmation { get; set; }


        public HelicopterTaskViewModel()
        {
            HasTranslationSupport = false;
            Helicopter = string.Empty;
            NoiseProtection = string.Empty;
            WorkInterval = string.Empty;

            //UrlCreateTranslation = string.Empty;
            //UrlEditTranslation = string.Empty;
            //UrlDeleteTranslationConfirmation = string.Empty;
            //Translations = new List<GenericTranslationViewModel>();
        }
    }
}