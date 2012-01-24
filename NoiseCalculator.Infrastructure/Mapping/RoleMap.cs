using FluentNHibernate.Mapping;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.Mapping
{
    public class RoleMap : ClassMap<Role>
    {
        public RoleMap()
        {
            Id(x => x.Id);

            Map(x => x.Title);
            Map(x => x.RoleType);
        }
    }
}
