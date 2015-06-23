using System.Collections.Generic;
using NoiseCalculator.UI.Web.Areas.Admin.Resources;
using NoiseCalculator.UI.Web.ViewModels;

namespace NoiseCalculator.UI.Web.Areas.Admin.ViewModels.TaskDefinition
{
    public class NewTaskDefinitionViewModel
    {
        public int Id { get; set; }
        public string SystemName { get; set; }
        public IList<SelectOptionViewModel> RoleTypes { get; set; }
        public string SystemNameEN { get; set; }


        public NewTaskDefinitionViewModel()
        {
            RoleTypes = new List<SelectOptionViewModel>();
            AddRoleTypes();
        }

        private void AddRoleTypes()
        {
            RoleTypes.Add(new SelectOptionViewModel(AdminResources.RoleTypeAreaNoise, "AreaNoise"));
            RoleTypes.Add(new SelectOptionViewModel(AdminResources.RoleTypeHelideck, "Helideck"));
            RoleTypes.Add(new SelectOptionViewModel(AdminResources.RoleTypeRegular, "Regular"){ IsSelected = true });
            RoleTypes.Add(new SelectOptionViewModel(AdminResources.RoleTypeRotation, "Rotation"));
        }
    }
}