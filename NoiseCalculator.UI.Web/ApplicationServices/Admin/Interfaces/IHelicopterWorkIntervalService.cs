using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.Generic;

namespace NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces
{
    public interface IHelicopterWorkIntervalService
    {
        GenericDefinitionIndexViewModel Index();
        GenericDefinitionViewModel Create(GenericDefinitionEditModel editModel);
        GenericDefinitionViewModel EditHelicopterTypeForm(int id);
        GenericDefinitionViewModel Edit(int id, GenericDefinitionEditModel editModel);
        DeleteConfirmationViewModel DeleteConfirmationForm(int id);
        void Delete(int id);
    }
}
