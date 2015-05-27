using FluentNHibernate.Mapping;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.Mapping
{
    public class NoiseProtectionMap : ClassMap<NoiseProtection>
    {
        public NoiseProtectionMap()
        {
            Id(x => x.Id);

            Map(x => x.Title);
            Map(x => x.CultureName);
            Map(x => x.NoiseDampening);

            References(x => x.NoiseProtectionDefinition).Nullable();
        }
    }
}
