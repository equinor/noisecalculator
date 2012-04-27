using FluentNHibernate.Mapping;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.Mapping
{
    public class HelicopterNoiseProtectionDefinitionMap : ClassMap<HelicopterNoiseProtectionDefinition>
    {
        public HelicopterNoiseProtectionDefinitionMap()
        {
            Id(x => x.Id);

            Map(x => x.SystemName);

            HasMany(x => x.HelicopterNoiseProtections)
                .Cascade.All();
        }
    }
}
