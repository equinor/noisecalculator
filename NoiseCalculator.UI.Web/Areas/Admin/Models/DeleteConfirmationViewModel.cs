using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.UI.Web.ViewModels
{
    public class DeleteConfirmationViewModel
    {
        public int Id { get; set; }
        public string SystemName { get; set; }

        public DeleteConfirmationViewModel(NoiseProtectionDefinition noiseProtectionDefinition)
        {
            Id = noiseProtectionDefinition.Id;
            SystemName = noiseProtectionDefinition.SystemName;
        }
    }
}