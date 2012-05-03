using FluentNHibernate.Mapping;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.Mapping
{
    public class HelicopterTaskDefinitionMap : ClassMap<HelicopterTaskDefinition>
    {
        public HelicopterTaskDefinitionMap()
        {
            Id(x => x.Id);

            Map(x => x.Title);

            HasMany(x => x.HelicopterTasks)
                .Cascade.AllDeleteOrphan();
        }
    }
}
