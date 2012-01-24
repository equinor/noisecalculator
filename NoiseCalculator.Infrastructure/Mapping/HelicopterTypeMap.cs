using FluentNHibernate.Mapping;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.Mapping
{
    public class HelicopterTypeMap : ClassMap<HelicopterType>
    {
        public HelicopterTypeMap()
        {
            Id(x => x.Id);

            Map(x => x.Title);
        }
    }
}
