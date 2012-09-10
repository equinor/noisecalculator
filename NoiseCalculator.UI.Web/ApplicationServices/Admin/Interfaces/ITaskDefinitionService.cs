using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.Models.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.Models.TaskDefinition;

namespace NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces
{
    public interface ITaskDefinitionService
    {
        TaskDefinitionIndexViewModel Index();
        TaskDefinitionListItemViewModel Create(CreateTaskDefinitionEditModel editModel);
        DeleteConfirmationViewModel DeleteConfirmationForm(int id);
        void Delete(int id);
    }
}
