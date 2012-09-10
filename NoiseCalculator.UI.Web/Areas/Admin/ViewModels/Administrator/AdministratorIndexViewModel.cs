using System.Collections.Generic;

namespace NoiseCalculator.UI.Web.Areas.Admin.ViewModels.Administrator
{
    public class AdministratorIndexViewModel
    {
        public IList<AdministratorListItemViewModel> Administrators { get; private set; }

        public AdministratorIndexViewModel()
        {
            Administrators = new List<AdministratorListItemViewModel>();
        }
    }
}