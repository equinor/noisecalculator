using System.Collections.Generic;

namespace NoiseCalculator.UI.Web.Areas.Admin.Models.Task
{
    public class TaskDefinitionViewModel
    {
        public int Id { get; set; }
        public string SystemName{ get; set; }
        public IList<TaskListItemViewModel> Tasks { get; set; }

        public string UrlCreateTranslation { get; set; }
        public string UrlEditTranslation { get; set; }
        public string UrlDeleteTranslationConfirmation { get; set; }


        public TaskDefinitionViewModel()
        {
            SystemName = string.Empty;
            Tasks = new List<TaskListItemViewModel>();

            UrlCreateTranslation = string.Empty;
            UrlEditTranslation = string.Empty;
            UrlDeleteTranslationConfirmation = string.Empty;
        }
    }
}