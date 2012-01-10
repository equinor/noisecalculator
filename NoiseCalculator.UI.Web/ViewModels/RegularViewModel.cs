using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoiseCalculator.UI.Web.ViewModels
{
    public class RegularViewModel
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Role { get; set; }
        public string NoiseLevelGuideline { get; set; }
        public bool IsNoiseMeassured { get; set; }
        public string NoiseLevelMeassured { get; set; }

        public bool IsWorkSpecifiedAsTime { get; set; }
        public bool IsWorkSpecifiedAsPercentage { get; set; }
        public string Hours { get; set; }
        public string Minutes { get; set; }
        public string Percentage { get; set; }
    }
}