using System.Collections.Generic;

namespace NoiseCalculator.UI.Web.Areas.Admin.Models
{
    public class GenericDefinitionIndexViewModel
    {
        public string PageTitle { get; set; }        
        public IList<GenericDefinitionViewModel> Definitions { get; private set; }
        public string UrlCreate { get; set; }
        public string UrlEdit { get; set; }
        public string UrlDeleteConfirmation { get; set; }
        

        public GenericDefinitionIndexViewModel()
        {
            Definitions = new List<GenericDefinitionViewModel>();
        }
    }
}