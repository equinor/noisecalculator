using System.Collections.Generic;
using System.Linq;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Domain.Enums;
using NoiseCalculator.Domain.Resources;

namespace NoiseCalculator.Domain.DomainServices
{
    public class FootnotesService : IFootnotesService
    {
        public IEnumerable<string> CalculateFootnotes(IEnumerable<SelectedTask> selectedTasks)
        {
            List<string> footnotes = GetStaticFootnotes();
            //footnotes.AddRange(CalculateDynamicFootnotes(selectedTasks));
            
            return footnotes;
        }
        
        private List<string> GetStaticFootnotes()
        {
            List<string> staticFootnotes = new List<string>
                {
                    DomainResources.FooterCalculations,
                    DomainResources.FooterNoiseProtection,
                    DomainResources.FooterContact
                };

            return staticFootnotes;
        }

        private IEnumerable<string> CalculateDynamicFootnotes(IEnumerable<SelectedTask> selectedTasks)
        {
            bool hasNoisyWork = (selectedTasks.Any(x => x.Task.Role.RoleType != RoleTypeEnum.AreaNoise));
            bool hasRegularTasks = (hasNoisyWork && selectedTasks.Any(x => x.HelicopterTaskId == 0));

            IList<string> dynamicFootnotes = new List<string>();

            if (hasNoisyWork)
            {
                dynamicFootnotes.Add(DomainResources.FooterDynamicNoiseProtection);
                dynamicFootnotes.Add(DomainResources.FooterDynamicCorrectionForMeasuredNoiseLevel);

                if (hasRegularTasks)
                {
                    dynamicFootnotes.Add(DomainResources.FooterDynamicValidForAreaNoiseUpTo90dBA);
                }
            }

            return dynamicFootnotes;
        }
    }
}
