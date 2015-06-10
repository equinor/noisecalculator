namespace NoiseCalculator.UI.Web.Areas.Admin.ViewModels.HelicopterTask
{
    public class HelicopterTaskListItemViewModel
    {
        public int Id { get; set; }
        public string Helicopter { get; set; }
        public string NoiseProtectionDefinition { get; set; }
        public int ButtonPressed { get; set; }
        public int FixedTime { get; set; }
    }
}