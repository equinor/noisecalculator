using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.RotationTask;

namespace NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces
{
    public interface IRotationTaskDefinitionService
    {
        TaskDefinitionRotationViewModel EditGenericTaskDefinitionForm(int id);
    }
}
