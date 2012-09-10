using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.RotationTask;

namespace NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces
{
    public interface IRotationTaskService
    {
        RotationTaskViewModel CreateRotationTaskForm(int definitionId);
        RotationTaskListItemViewModel Create(RotationTaskEditModel editModel);
        RotationTaskViewModel EditRotationTaskForm(int id);
        RotationTaskListItemViewModel Edit(int id, RotationTaskEditModel editModel);
        DeleteConfirmationViewModel DeleteConfirmationForm(int id);
        void Delete(int id);
    }
}
