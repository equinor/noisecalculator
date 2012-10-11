using System.Collections.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.Resources;

namespace NoiseCalculator.UI.Web.Areas.Admin.EditModels
{
    public class TaskEditModel : EditModelBase
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int RoleId { get; set; }
        public int NoiseProtectionId { get; set; }
        public string NoiseLevelGuideline { get; set; }
        public string Hours { get; set; }
        public string Minutes { get; set; }

        public string SelectedCultureName { get; set; }
        public int DefinitionId { get; set; }


        public bool IsValid()
        {
            return IsValid(Validate);
        }

        public IList<string> GetValidationErrors()
        {
            return GetValidationErrors(Validate);
        }

        private IList<string> Validate()
        {
            IList<string> validationErrors = new List<string>();

            if (string.IsNullOrEmpty(Title))
            {
                validationErrors.Add(AdminResources.ValidationTitleRequired);
            }

            if (RoleId <= 0)
            {
                validationErrors.Add(AdminResources.ValidationRoleRequired);
            }

            if (NoiseProtectionId <= 0)
            {
                validationErrors.Add(AdminResources.ValidationTaskEditModelNoiseProtectionRequired);
            }

            if (string.IsNullOrEmpty(NoiseLevelGuideline))
            {
                validationErrors.Add(AdminResources.ValidationTaskEditModelNoiseLevelRequired);
            }

            if (string.IsNullOrEmpty(Hours) && string.IsNullOrEmpty(Minutes))
            {
                validationErrors.Add(AdminResources.ValidationTaskEditModelAllowedExposureRequired);
            }

            if (string.IsNullOrEmpty(SelectedCultureName) || SelectedCultureName == "0")
            {
                validationErrors.Add(AdminResources.ValidationLanguageRequired);
            }

            if (DefinitionId <= 0)
            {
                validationErrors.Add(AdminResources.ValidationDefinitionIdRequired);
            }

            return validationErrors;
        }
    }
}