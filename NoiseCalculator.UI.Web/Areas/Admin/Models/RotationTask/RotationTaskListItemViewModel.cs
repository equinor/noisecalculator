namespace NoiseCalculator.UI.Web.Areas.Admin.Models.RotationTask
{
    public class RotationTaskListItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } // Task.Title
        public string OperatorTask { get; set; }
        public string OperatorRole { get; set; }
        public string AssistantTask { get; set; }
        public string AssistantRole { get; set; }
    }
}