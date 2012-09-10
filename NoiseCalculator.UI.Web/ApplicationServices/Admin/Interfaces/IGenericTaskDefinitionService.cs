using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.GenericTask;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.TaskDefinition;

namespace NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces
{
    public interface IGenericTaskDefinitionService
    {
        TaskDefinitionGenericViewModel EditGenericTaskDefinitionForm(int id);
        TaskDefinitionListItemViewModel Edit(int id, GenericDefinitionEditModel editModel);
    }
}
