using System;
using System.Collections.Generic;

namespace NoiseCalculator.Infrastructure.Pdf
{
    public class ReportInfo
    {
        public string Plant { get; set; }
        public string Group { get; set; }
        public DateTime? Date { get; set; }
        public string CreatedBy { get; set; }
        public string Comment { get; set; }
        public IEnumerable<string> Footnotes { get; set; }
    }
}