using System.Collections.Generic;

namespace NoiseCalculator.UI.Web.Areas.Admin.Models.Administrator
{
    public class AdministratorIndexViewModel
    {
        //public string PageTitle { get; set; }        
        public IList<AdministratorListItemViewModel> Administrators { get; private set; }
        public string UrlCreate { get; set; }
        //public string UrlEdit { get; set; }
        public string UrlDeleteConfirmation { get; set; }

        public AdministratorIndexViewModel()
        {
            Administrators = new List<AdministratorListItemViewModel>();
        }
    }
}