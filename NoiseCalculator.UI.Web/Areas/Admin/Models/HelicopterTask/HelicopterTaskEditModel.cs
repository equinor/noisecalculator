namespace NoiseCalculator.UI.Web.Areas.Admin.Models.HelicopterTask
{
    public class HelicopterTaskEditModel
    {
        public int Id { get; set; }
        public int HelicopterTypeId { get; set; }
        public int HelicopterNoiseProtectionDefinitionId { get; set; }
        public int HelicopterWorkIntervalId { get; set; }
        public int Percentage { get; set; }
    }
}