namespace NoiseCalculator.UI.Web.Areas.Admin.ViewModels.GenericTask
{
    public class TaskListItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Role { get; set; }
        public string NoiseProtection { get; set; }
        public decimal NoiseLevelGuideline { get; set; }
        public int AllowedExposureMinutes { get; set; }
        public int ButtonPressed { get; set; }
        public int BackgroundNoise { get; set; }
        public string Language { get; set; }
    }
}