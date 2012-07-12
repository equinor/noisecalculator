namespace NoiseCalculator.UI.Web.Areas.Admin.Models.GenericTask
{
    public class TaskEditModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int RoleId { get; set; }
        public int NoiseProtectionId { get; set; }
        public int NoiseLevelGuideline { get; set; }
        public int AllowedExposureMinutes { get; set; }

        public string SelectedCultureName { get; set; }
        public int DefinitionId { get; set; }
    }
}