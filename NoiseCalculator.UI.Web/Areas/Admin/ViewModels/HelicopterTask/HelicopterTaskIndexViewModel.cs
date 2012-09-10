using System.Collections.Generic;

namespace NoiseCalculator.UI.Web.Areas.Admin.ViewModels.HelicopterTask
{
    public class HelicopterTaskIndexViewModel
    {
        public IList<HelicopterTaskListItemViewModel> HelicopterTasks { get; private set; }

        public HelicopterTaskIndexViewModel()
        {
            HelicopterTasks = new List<HelicopterTaskListItemViewModel>();
        }
    }
}