using System.Collections.Generic;

namespace NoiseCalculator.UI.Web.Resources
{
    public static class FootnoteResources
    {
        public static IEnumerable<string> GetStaticFootnotes()
        {
            string[] staticFootnotes = new[] 
                { 
                    TaskResources.FooterGL0169, 
                    TaskResources.FooterNoiseProtectionDefinition,
                    TaskResources.FooterDynamicUseNoisyWorkWhenTaskIsMissing,
                    TaskResources.Footer80dBA
                };
            
            return staticFootnotes;
        }
    }
}