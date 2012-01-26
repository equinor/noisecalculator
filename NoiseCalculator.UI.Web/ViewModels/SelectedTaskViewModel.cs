namespace NoiseCalculator.UI.Web.ViewModels
{
    public class SelectedTaskViewModel
    {
        public string Title { get; set; }
        public string Role { get; set; }
        public string NoiseProtection { get; set; }
        public string NoiseLevel { get; set; }
        public string Hours { get; set; }
        public string Minutes { get; set; }
        public string Percentage { get; set; }

        public int TaskId { get; set; }
        public int HelicopterTaskId { get; set; }


        public SelectedTaskViewModel()
        {
            Title = string.Empty;
            Role = string.Empty;
            NoiseProtection = string.Empty;
            NoiseLevel = string.Empty;
            Hours = string.Empty;
            Minutes = string.Empty;
            Percentage = string.Empty;
        }
    }
}