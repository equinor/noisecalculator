using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.Models.Administrator;
using NoiseCalculator.UI.Web.Areas.Admin.Models.Generic;

namespace NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces
{
    public interface IAdministratorService
    {
        AdministratorIndexViewModel Index();
        AdministratorListItemViewModel Create(AdministratorEditModel editModel);
        DeleteConfirmationViewModel DeleteConfirmationForm(string id);
        void Delete(string id);
    }
}
