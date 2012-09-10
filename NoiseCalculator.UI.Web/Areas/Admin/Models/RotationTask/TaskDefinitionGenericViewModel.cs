using System.Collections.Generic;

namespace NoiseCalculator.UI.Web.Areas.Admin.Models.RotationTask
{
    public class TaskDefinitionRotationViewModel
    {
        public int Id { get; set; }
        public string SystemName { get; set; }
        public string RoleType { get; set; }

        public IList<RotationTaskListItemViewModel> RotationTasks { get; set; }

        //public string UrlCreateTranslation { get; set; }
        //public string UrlEditTranslation { get; set; }
        //public string UrlDeleteTranslationConfirmation { get; set; }


        public TaskDefinitionRotationViewModel()
        {
            RotationTasks = new List<RotationTaskListItemViewModel>();
            //UrlCreateTranslation = string.Empty;
            //UrlEditTranslation = string.Empty;
            //UrlDeleteTranslationConfirmation = string.Empty;
        }
    }
}