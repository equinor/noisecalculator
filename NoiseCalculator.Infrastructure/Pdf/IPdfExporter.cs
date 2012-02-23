using System.Collections.Generic;
using System.IO;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.Pdf
{
    public interface IPdfExporter
    {
        Stream GenerateSelectedTasksPDF(IEnumerable<SelectedTask> selectedTasks);
    }
}
