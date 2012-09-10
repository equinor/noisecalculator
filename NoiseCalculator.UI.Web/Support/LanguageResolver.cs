using NoiseCalculator.UI.Web.Resources;

namespace NoiseCalculator.UI.Web.Support
{
    public class LanguageResolver
    {
        public static string GetLanguageName(string cultureName)
        {
            switch (cultureName)
            {
                case "nb-NO":
                    {
                        return TaskResources.LanguageNameNorwegian;
                    }
                case "en-US":
                    {
                        return TaskResources.LanguageNameEnglish;
                    }
                default:
                    {
                        return string.Format("{0} - {1}", TaskResources.LanguageNameUnknown, cultureName);
                    }
            }
        }
    }
}