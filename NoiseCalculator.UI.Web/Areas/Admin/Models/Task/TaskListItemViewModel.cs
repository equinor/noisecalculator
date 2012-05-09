namespace NoiseCalculator.UI.Web.Areas.Admin.Models.Task
{
    public class TaskListItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Role { get; private set; }
        public string NoiseProtection { get; private set; }
        public int NoiseLevelGuideline { get; set; }
        public int AllowedExposureMinutes { get; set; }

        public string Language { get; private set; }
    }
}