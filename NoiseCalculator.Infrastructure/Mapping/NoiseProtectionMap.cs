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
            //Join("NoiseProtectionTranslation",
            //     x =>
            //     {
            //         x.KeyColumn("NoiseProtection_id");
            //         x.Map(m => m.Title);
            //     });
            //ApplyFilter<CultureNameFilterDefinition>();
            Map(x => x.CultureName);

            References(x => x.NoiseProtectionDefinition).Not.Nullable();
        }
    }
}
