using System.Collections.Generic;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.ViewModels.TaskDefinition
{
    public class NewTaskDefinitionViewModel
    {
        public int Id { get; set; }
        public string SystemName { get; set; }
        public IList<SelectOptionViewModel> RoleTypes { get; set; }

        
        public NewTaskDefinitionViewModel()
        {
            RoleTypes = new List<SelectOptionViewModel>();
            AddRoleTypes();
        }

        private void AddRoleTypes()
        {
            // Translation must be added
            RoleTypes.Add(new SelectOptionViewModel("Area Noise", "AreaNoise"));
            RoleTypes.Add(new SelectOptionViewModel("Helideck", "Helideck"));
            RoleTypes.Add(new SelectOptionViewModel("Regular", "Regular"){ IsSelected = true });
            RoleTypes.Add(new SelectOptionViewModel("Rotation", "Rotation"));
        }
    }
}