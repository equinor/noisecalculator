using System.Collections.Generic;

namespace NoiseCalculator.UI.Web.ViewModels
{
    public class TaskIndexViewModel
    {
        public TaskIndexViewModel()
        {
            SelectedTasks = new List<SelectedTaskViewModel>();
        }

        public IList<SelectedTaskViewModel> SelectedTasks { get; private set; }
        public bool IsAdmin { get; set; }
        public bool IsLoggedIn { get; set; }
    }
}