using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.Generic;

namespace NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces
{
    public interface INoiseProtectionService
    {
        GenericTranslationViewModel CreateNoiseProtectionForm(int definitionId);
        GenericTranslationViewModel Create(GenericTranslationEditModel editModel);
        GenericTranslationViewModel EditNoiseProtectionForm(int id);
        GenericTranslationViewModel Edit(GenericTranslationEditModel editModel);
        DeleteConfirmationViewModel DeleteConfirmationForm(int id);
        void Delete(int id);
    }
}