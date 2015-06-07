using FluentNHibernate.Mapping;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.Mapping
{
    public class TaskMap : ClassMap<Task>
    {
        public TaskMap()
        {
            Id(x => x.Id);

            Map(x => x.Title);
            Map(x => x.NoiseLevelGuideline);
            Map(x => x.AllowedExposureMinutes);
            Map(x => x.CultureName);
            Map(x => x.SortOrder);
            Map(x => x.ButtonPressed);

            References(x => x.Role);
            References(x => x.NoiseProtection);
            References(x => x.TaskDefinition);
        }
    }
}
