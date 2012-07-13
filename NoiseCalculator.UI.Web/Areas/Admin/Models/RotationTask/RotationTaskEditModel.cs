namespace NoiseCalculator.UI.Web.Areas.Admin.Models.RotationTask
{
    public class RotationTaskEditModel
    {
        public string SelectedCultureName { get; set; }
        public string Title { get; set; }
        public int OperatorTaskId { get; set; }
        public int AssistantTaskId { get; set; }
        public int TaskDefinitionId { get; set; }
    }
}