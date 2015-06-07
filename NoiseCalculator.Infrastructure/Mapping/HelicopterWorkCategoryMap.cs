using FluentNHibernate.Mapping;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.Mapping
{
    public class HelicopterWorkCategoryMap : ClassMap<HelicopterWorkCategory>
    {
        public HelicopterWorkCategoryMap()
        {
            Id(x => x.Id);

            Map(x => x.Title);
        }
    }
}
