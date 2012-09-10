using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.GenericTask;

namespace NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces
{
    public interface IGenericTaskService
    {
        TaskViewModel CreateGenericTaskForm(int definitionId);
        TaskListItemViewModel Create(TaskEditModel editModel);
        TaskViewModel EditGenericTaskForm(int id);
        TaskListItemViewModel Edit(TaskEditModel editModel);
        DeleteConfirmationViewModel DeleteConfirmationForm(int id);
        void Delete(int id);
    }
}
