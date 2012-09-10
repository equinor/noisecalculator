using System.Collections.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.Resources;

namespace NoiseCalculator.UI.Web.Areas.Admin.EditModels
{
    public class CreateTaskDefinitionEditModel : EditModelBase
    {
        public string Title { get; set; }
        public string RoleType { get; set; }

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

            if (string.IsNullOrEmpty(RoleType))
            {
                validationErrors.Add(AdminResources.ValidationRoleRequired);
            }

            return validationErrors;
        }
    }
}