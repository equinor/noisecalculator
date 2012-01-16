using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoiseCalculator.UI.Web.ViewModels
{
    public class HelideckViewModel
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Role { get; set; }
        public int HelicopterId { get; set; }
        public int NoiseProtectionId { get; set; }
        public int WorkIntervalId { get; set; }

        public SelectList Helicopters { get; set; }
        public SelectList NoiseProtection { get; set; }
        public SelectList WorkIntervals { get; set; }
    }
}