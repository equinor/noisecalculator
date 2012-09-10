using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.Generic;

namespace NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces
{
    public interface IHelicopterNoiseProtectionService
    {
        GenericTranslationViewModel CreateHelicopterNoiseProtectionForm(int id);
        GenericTranslationViewModel Create(GenericTranslationEditModel editModel);        
        GenericTranslationViewModel EditHelicopterNoiseProtiectionForm(int id);
        GenericTranslationViewModel Edit(GenericTranslationEditModel editModel);
        DeleteConfirmationViewModel DeleteConfirmationForm(int id);
        void Delete(int id);
    }
}