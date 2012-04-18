namespace NoiseCalculator.UI.Web.Areas.Admin.Models
{
    public class GenericDefinitionViewModel
    {
        public int Id { get; set; }
        public string SystemName { get; set; }

        public GenericDefinitionViewModel()
        {
            SystemName = string.Empty;
        }
    }
}