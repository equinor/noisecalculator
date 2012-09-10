using System.Collections.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.Resources;

namespace NoiseCalculator.UI.Web.Areas.Admin.EditModels
{
    public class GenericTranslationEditModel : EditModelBase
    {
        public int Id { get; set; }
        public string Title { get; set; }
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
            
            if (string.IsNullOrEmpty(SelectedCultureName) || SelectedCultureName == "0")
            {
                validationErrors.Add(AdminResources.ValidationLanguageRequired);
            }

            if (string.IsNullOrEmpty(Title))
            {
                validationErrors.Add(AdminResources.ValidationTitleRequired);
            }

            return validationErrors;
        }
    }
}