namespace NoiseCalculator.UI.Web.ViewModels
{
    public class RotationViewModel
    {
        //public int TaskId { get; set; }
        public int RotationId { get; set; }
        public int SelectedTaskId { get; set; }
        public string RoleType { get; set; }
        public string Title { get; set; }
        
        public string OperatorNoiseLevelGuideline { get; set; }
        public string OperatorTitle { get; set; }
        public string AssistantNoiseLevelGuideline { get; set; }
        public string AssistantTitle { get; set; }
        
        public string RadioNoiseMeassuredNoCheckedAttr { get; set; }
        public string RadioNoiseMeassuredYesCheckedAttr { get; set; }

        public string NoiseLevelMeassured { get; set; }
        public string RadioTimeCheckedAttr { get; set; }
        public string RadioPercentageCheckedAttr { get; set; }

        public string Hours { get; set; }
        public string Minutes { get; set; }
        public string Percentage { get; set; }
    }
}
