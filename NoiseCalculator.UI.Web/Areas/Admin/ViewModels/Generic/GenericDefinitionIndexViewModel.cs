using System.Collections.Generic;

namespace NoiseCalculator.UI.Web.Areas.Admin.ViewModels.Generic
{
    public class GenericDefinitionIndexViewModel
    {
        public IList<GenericDefinitionViewModel> Definitions { get; private set; }

        public GenericDefinitionIndexViewModel()
        {
            Definitions = new List<GenericDefinitionViewModel>();
        }
    }
}