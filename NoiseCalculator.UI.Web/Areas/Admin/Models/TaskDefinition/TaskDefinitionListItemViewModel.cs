namespace NoiseCalculator.UI.Web.Areas.Admin.Models.TaskDefinition
{
    public class TaskDefinitionListItemViewModel
    {
        public int Id { get; set; }
        public string SystemName { get; set; }
        public string RoleType { get; set; }


        public TaskDefinitionListItemViewModel()
        {
            SystemName = string.Empty;
            RoleType = string.Empty;
        }
    }
}