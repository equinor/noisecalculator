using System.Collections.Generic;
using NoiseCalculator.UI.Web.Resources;

namespace NoiseCalculator.UI.Web.Areas.Admin.EditModels
{
    public class HelicopterTaskEditModel : EditModelBase
    {
        public int Id { get; set; }
        public int HelicopterTypeId { get; set; }
        public decimal NoiseLevel { get; set; }
        public int TaskId { get; set; }

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

            if (TaskId == 0)
            {
                validationErrors.Add(TaskResources.ValidationErrorHelicopterNoiseLevelRequired);
            }

            
            return validationErrors;
        }
    }
}