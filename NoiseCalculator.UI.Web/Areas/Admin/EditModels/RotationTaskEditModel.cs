using System.Collections.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.Resources;

namespace NoiseCalculator.UI.Web.Areas.Admin.EditModels
{
    public class RotationTaskEditModel : EditModelBase
    {
        public string SelectedCultureName { get; set; }
        public string Title { get; set; }
        public int OperatorTaskId { get; set; }
        public int AssistantTaskId { get; set; }
        public int TaskDefinitionId { get; set; }

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

            // TRANSLATIONS
            if (string.IsNullOrEmpty(SelectedCultureName) || SelectedCultureName == "0")
            {
                validationErrors.Add(AdminResources.ValidationLanguageRequired);
            }
            
            if (string.IsNullOrEmpty(Title))
            {
                validationErrors.Add(AdminResources.ValidationTitleRequired);
            }

            if (OperatorTaskId <= 0)
            {
                validationErrors.Add(AdminResources.ValidationRotationTaskEditModelIOperatorTaskRequired);
            }

            if (AssistantTaskId <= 0)
            {
                validationErrors.Add(AdminResources.ValidationRotationTaskEditModelIAssistantTaskRequired);
            }

            if (TaskDefinitionId <= 0)
            {
                validationErrors.Add(AdminResources.ValidationDefinitionIdRequired);
            }

            return validationErrors;
        }
    }
}