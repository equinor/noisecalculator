using FluentNHibernate.Mapping;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.Mapping
{
    public class HelicopterTaskMap : ClassMap<HelicopterTask>
    {
        public HelicopterTaskMap()
        {
            Id(x => x.Id);

            Map(x => x.Percentage);

            References(x => x.HelicopterType);
            References(x => x.HelicopterNoiseProtectionDefinition);
            References(x => x.HelicopterWorkInterval);
        }
    }
}
