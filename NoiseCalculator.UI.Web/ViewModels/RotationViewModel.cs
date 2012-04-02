namespace NoiseCalculator.UI.Web.ViewModels
{
    public class RotationViewModel
    {
        public int RotationId { get; set; }
        public int SelectedTaskId { get; set; }
        public string RoleType { get; set; }
        public string Title { get; set; }

        public string Hours { get; set; }
        public string Minutes { get; set; }
        public string Percentage { get; set; }
        
        public string OperatorNoiseLevelGuideline { get; set; }
        public string OperatorTitle { get; set; }
        public string OperatorNoiseLevelMeasured { get; set; } // Post property
        
        public string AssistantNoiseLevelGuideline { get; set; }
        public string AssistantTitle { get; set; }
        public string AssistantNoiseLevelMeasured { get; set; } // Post property

        public string RadioNoiseMeassuredNoCheckedAttr { get; set; }
        public string RadioNoiseMeassuredYesCheckedAttr { get; set; }
        public string RadioTimeCheckedAttr { get; set; }
        public string RadioPercentageCheckedAttr { get; set; }
    }
}
