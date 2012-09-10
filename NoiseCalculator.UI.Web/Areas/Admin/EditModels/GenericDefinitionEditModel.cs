using System.Collections.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.Resources;

namespace NoiseCalculator.UI.Web.Areas.Admin.EditModels
{
    public class GenericDefinitionEditModel : EditModelBase
    {
        public string Title { get; set; }

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

            return validationErrors;
        }
    }
}