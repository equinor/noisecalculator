using System.Collections.Generic;

namespace NoiseCalculator.UI.Web.Areas.Admin.ViewModels.RotationTask
{
    public class RotationTaskIndexViewModel
    {
        public string PageTitle { get; set; }        
        public IList<RotationTaskListItemViewModel> RotationTasks { get; private set; }
        public string UrlCreate { get; set; }
        public string UrlEdit { get; set; }
        public string UrlDeleteConfirmation { get; set; }

        public RotationTaskIndexViewModel()
        {
            RotationTasks = new List<RotationTaskListItemViewModel>();
        }
    }
}