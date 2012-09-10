using System.Collections.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.Resources;

namespace NoiseCalculator.UI.Web.Areas.Admin.EditModels
{
    public class AdministratorEditModel : EditModelBase
    {
        public string Username { get; set; }

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

            if (string.IsNullOrEmpty(Username))
            {
                validationErrors.Add(AdminResources.ValidationAdministratorEditModelUsername);
            }

            return validationErrors;
        }
    }
}