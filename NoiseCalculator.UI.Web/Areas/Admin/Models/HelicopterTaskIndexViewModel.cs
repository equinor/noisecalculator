using System.Collections.Generic;

namespace NoiseCalculator.UI.Web.Areas.Admin.Models
{
    public class HelicopterTaskIndexViewModel
    {
        public string PageTitle { get; set; }        
        public IList<HelicopterTaskViewModel> HelicopterTasks { get; private set; }
        public string UrlCreate { get; set; }
        public string UrlEdit { get; set; }
        public string UrlDeleteConfirmation { get; set; }

        public HelicopterTaskIndexViewModel()
        {
            HelicopterTasks = new List<HelicopterTaskViewModel>();
        }
    }
}