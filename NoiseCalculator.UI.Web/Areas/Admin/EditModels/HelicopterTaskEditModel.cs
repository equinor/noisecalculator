using System.Collections.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.Resources;
using NoiseCalculator.UI.Web.Resources;

namespace NoiseCalculator.UI.Web.Areas.Admin.EditModels
{
    public class HelicopterTaskEditModel : EditModelBase
    {
        public int Id { get; set; }
        public int HelicopterTypeId { get; set; }
        public int HelicopterNoiseProtectionDefinitionId { get; set; }
        public int HelicopterWorkIntervalId { get; set; }
        public int Percentage { get; set; }


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

            if (HelicopterTypeId == 0)
            {
                validationErrors.Add(TaskResources.ValidationErrorHelicopterTypeRequired);
            }

            if (HelicopterNoiseProtectionDefinitionId == 0)
            {
                validationErrors.Add(TaskResources.ValidationErrorHelicopterNoiseLevelRequired);
            }

            if (HelicopterWorkIntervalId == 0)
            {
                validationErrors.Add(TaskResources.ValidationErrorHelicopterWorkIntervalRequired);
            }

            if (Percentage <= 0)
            {
                validationErrors.Add(AdminResources.ValidationHelicopterTaskEditModelPercentage);
            }

            return validationErrors;
        }
    }
}