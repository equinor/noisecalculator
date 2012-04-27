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

            References(x => x.NoiseProtectionDefinition).Nullable();
        }
    }
}
