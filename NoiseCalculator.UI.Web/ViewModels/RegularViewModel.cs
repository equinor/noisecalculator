using System.Collections.Generic;
using System.Web.Mvc;

namespace NoiseCalculator.UI.Web.ViewModels
{
    public class RegularViewModel
    {
        public int TaskId { get; set; }
        public int SelectedTaskId { get; set; }
        public string Title { get; set; }
        public string Role { get; set; }
        public string RoleType { get; set; } // Its Value is derived from the RoleTypeEnum
        public string NoiseLevelGuideline { get; set; }
        public string RadioNoiseMeassuredNoCheckedAttr { get; set; }
        public string RadioNoiseMeassuredYesCheckedAttr { get; set; }
        
        public decimal NoiseLevelMeassured { get; set; }
        public string RadioTimeCheckedAttr { get; set; }
        public string RadioPercentageCheckedAttr { get; set; }
        
        public string Hours { get; set; }
        public string Minutes { get; set; }
        public string Percentage { get; set; }

        public string RotationDiplayStyle { get; set; }
        public int ButtonPressed { get; set; }
        public string BackgroundNoise { get; set; }
        public int NoiseProtectionId { get; set; }
        public IList<SelectListItem> NoiseProtection { get; set; }


        public RegularViewModel()
        {
            NoiseLevelGuideline = string.Empty;
            Hours = string.Empty;
            Minutes = string.Empty;
            Percentage = string.Empty;
            RadioNoiseMeassuredNoCheckedAttr = string.Empty;
            RadioNoiseMeassuredYesCheckedAttr = string.Empty;
            RadioTimeCheckedAttr = string.Empty;
            RadioPercentageCheckedAttr = string.Empty;
            RotationDiplayStyle = "style=\"display: none;\"";
            NoiseProtection = new List<SelectListItem>();
            BackgroundNoise = "<80";
        }
    }
}