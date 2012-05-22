using System.Collections.Generic;
using System.Linq;

namespace NoiseCalculator.UI.Web.Resources
{
    public static class FootnoteResources
    {
        private static readonly IList<string> StaticFootnotes = new List<string>();
        
        static FootnoteResources()
        {
            StaticFootnotes.Add(TaskResources.FooterGL0169);
            StaticFootnotes.Add(TaskResources.FooterNoiseProtectionDefinition);
            StaticFootnotes.Add(TaskResources.FooterDynamicUseNoisyWorkWhenTaskIsMissing);
            StaticFootnotes.Add(TaskResources.Footer80dBA);
        }
        
        public static IEnumerable<string> GetStaticFootnotes()
        {
            return StaticFootnotes.ToArray();
        }
    }
}