using FluentNHibernate.Mapping;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.Mapping
{
    public class HelicopterNoiseProtectionMap : ClassMap<HelicopterNoiseProtection>
    {
        public HelicopterNoiseProtectionMap()
        {
            Id(x => x.Id);

            Map(x => x.Title);
            //Join("HelicopterNoiseProtectionTranslation",
            //     x =>
            //     {
            //         x.KeyColumn("HelicopterNoiseProtection_id");
            //         x.Map(m => m.Title);
            //     });
            //ApplyFilter<CultureNameFilterDefinition>();
            Map(x => x.CultureName);

            References(x => x.HelicopterNoiseProtectionDefinition).Not.Nullable();
        }
    }
}
