using FluentNHibernate.Mapping;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.Mapping
{
    public class NoiseProtectionDefinitionMap : ClassMap<NoiseProtectionDefinition>
    {
        public NoiseProtectionDefinitionMap()
        {
            Id(x => x.Id);

            Map(x => x.SystemName);

            HasMany(x => x.NoiseProtections)
                .Cascade.AllDeleteOrphan()
                .Not.KeyNullable();
        }
    }
}
