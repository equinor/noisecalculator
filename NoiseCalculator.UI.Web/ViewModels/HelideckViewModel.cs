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
        public string HelicopterId { get; set; }
        public string NoiseProtectionId { get; set; }
        public string WorkIntervalId { get; set; }

        public SelectList Helicopters { get; set; }
        public SelectList NoiseProtection { get; set; }
        public SelectList WorkIntervals { get; set; }
    }
}