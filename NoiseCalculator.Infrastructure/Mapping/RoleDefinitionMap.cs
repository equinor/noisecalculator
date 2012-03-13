using FluentNHibernate.Mapping;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.Mapping
{
    public class RoleDefinitionMap : ClassMap<RoleDefinition>
    {
        public RoleDefinitionMap()
        {
            Id(x => x.Id);

            Map(x => x.SystemName);
        }
    }
}
