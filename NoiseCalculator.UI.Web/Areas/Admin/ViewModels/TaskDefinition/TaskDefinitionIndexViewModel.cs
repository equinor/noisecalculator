using System.Collections.Generic;

namespace NoiseCalculator.UI.Web.Areas.Admin.ViewModels.TaskDefinition
{
    public class TaskDefinitionIndexViewModel
    {
        //public string PageTitle { get; set; }        
        public IList<TaskDefinitionListItemViewModel> Definitions { get; private set; }
        //public string UrlCreate { get; set; }
        //public string UrlEditGeneric { get; set; }
        //public string UrlEditRotation { get; set; }
        //public string UrlDeleteConfirmation { get; set; }

        public TaskDefinitionIndexViewModel()
        {
            Definitions = new List<TaskDefinitionListItemViewModel>();
        }
    }
}