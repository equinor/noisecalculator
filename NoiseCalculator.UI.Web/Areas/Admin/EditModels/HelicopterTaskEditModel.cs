using System.Collections.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.Resources;
using NoiseCalculator.UI.Web.Resources;

namespace NoiseCalculator.UI.Web.Areas.Admin.EditModels
{
    public class HelicopterTaskEditModel : EditModelBase
    {
        public int Id { get; set; }
        public int HelicopterTypeId { get; set; }
        public int NoiseProtectionDefinitionId { get; set; }
        public int ButtonPressed { get; set; }
        public int FixedTime { get; set; }

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

            if (NoiseProtectionDefinitionId == 0)
            {
                validationErrors.Add(TaskResources.ValidationErrorHelicopterNoiseLevelRequired);
            }
            
            return validationErrors;
        }
    }
}