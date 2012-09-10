using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.Models.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.Models.HelicopterTask;

namespace NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces
{
    public interface IHelicopterTaskService
    {
        HelicopterTaskIndexViewModel Index();
        HelicopterTaskViewModel CreateNoiseProtectionForm();
        HelicopterTaskListItemViewModel Create(HelicopterTaskEditModel editModel);
        HelicopterTaskViewModel EditNoiseProtectionForm(int id);
        HelicopterTaskListItemViewModel Edit(int id, HelicopterTaskEditModel editModel);
        DeleteConfirmationViewModel DeleteConfirmationForm(int id);
        void Delete(int id);
    }
}
