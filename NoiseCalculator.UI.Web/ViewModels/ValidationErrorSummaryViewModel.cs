using System.Collections.Generic;

namespace NoiseCalculator.UI.Web.ViewModels
{
    public class ValidationErrorSummaryViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IList<string> ValidationErrors { get; set; }

        public ValidationErrorSummaryViewModel()
        {
            ValidationErrors = new List<string>();
        }
    }
}