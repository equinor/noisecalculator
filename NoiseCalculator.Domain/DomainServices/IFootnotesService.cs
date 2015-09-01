using System.Collections.Generic;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Domain.DomainServices
{
    public interface IFootnotesService
    {
        IEnumerable<string> CalculateFootnotes(IEnumerable<SelectedTask> selectedTasks);
        IEnumerable<string> CalculateFootnotesForReport(IEnumerable<SelectedTask> selectedTasks);
    }
}