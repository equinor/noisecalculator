namespace NoiseCalculator.UI.Web.Areas.Admin.ViewModels.Administrator
{
    public class AdministratorViewModel
    {
        public string Username { get; set; }
        public bool HasTranslationSupport { get; set; } // ???????

        public AdministratorViewModel()
        {
            HasTranslationSupport = true; // ???????
            Username = string.Empty;
        }
    }
}