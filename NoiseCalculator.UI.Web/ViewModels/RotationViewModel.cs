namespace NoiseCalculator.UI.Web.ViewModels
{
    public class RotationViewModel
    {
        public int TaskId { get; set; }
        public int SelectedTaskId { get; set; }
        public string Title { get; set; }
        public string Role { get; set; }
        public string RoleType { get; set; } // Its Value is derived from the RoleTypeEnum
        public string NoiseLevelGuideline { get; set; }
        public string RadioNoiseMeassuredNoCheckedAttr { get; set; }
        public string RadioNoiseMeassuredYesCheckedAttr { get; set; }
        
        public string NoiseLevelMeassured { get; set; }
        public string RadioTimeCheckedAttr { get; set; }
        public string RadioPercentageCheckedAttr { get; set; }
        
        public string Hours { get; set; }
        public string Minutes { get; set; }
        public string Percentage { get; set; }

        public string HoursAssisant { get; set; }
        public string MinutesAssisant { get; set; }
        public string PercentageAssisant { get; set; }


        public RotationViewModel()
        {
            NoiseLevelGuideline = string.Empty;
            NoiseLevelMeassured = string.Empty;
            Hours = string.Empty;
            Minutes = string.Empty;
            Percentage = string.Empty;
            RadioNoiseMeassuredNoCheckedAttr = string.Empty;
            RadioNoiseMeassuredYesCheckedAttr = string.Empty;
            RadioTimeCheckedAttr = string.Empty;
            RadioPercentageCheckedAttr = string.Empty;

            HoursAssisant = string.Empty;
            MinutesAssisant = string.Empty;
            PercentageAssisant = string.Empty;

            //IsNoiseMeassured = string.Empty;
            //IsWorkSpecifiedAsTime { get; set; }
            //IsWorkSpecifiedAsPercentage { get; set; }
        }
    }
}