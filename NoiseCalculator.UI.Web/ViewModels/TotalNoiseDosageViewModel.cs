using System.Collections.Generic;

namespace NoiseCalculator.UI.Web.ViewModels
{
    public class TotalNoiseDosageViewModel
    {
        public int Percentage { get; set; }
        public string StatusText { get; set; }
        public string CssClass { get; set; }
        public IEnumerable<string> Footnotes { get; set; }
    }
}