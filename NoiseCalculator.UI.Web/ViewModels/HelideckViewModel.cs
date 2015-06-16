using System.Collections.Generic;
using System.Web.Mvc;

namespace NoiseCalculator.UI.Web.ViewModels
{
    public class HelideckViewModel
    {
        public decimal ActualNoiseLevel { get; set; }
        public int TaskId { get; set; }
        public int SelectedTaskId { get; set; }
        public string Title { get; set; }
        public string Role { get; set; }
        public string RoleType { get; set; } // Its Value is derived from the RoleTypeEnum
        public int HelicopterId { get; set; }
        public int NoiseProtectionId { get; set; }

        public IList<SelectListItem> Helicopters { get; set; }
        public IList<SelectListItem> NoiseProtection { get; set; }
        public IList<SelectListItem> Task { get; set; }
        public decimal NoiseLevel { get; set; }


        public HelideckViewModel()
        {
            Helicopters = new List<SelectListItem>();
            NoiseProtection = new List<SelectListItem>();
        }
    }
}