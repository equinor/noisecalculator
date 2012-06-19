using FluentNHibernate.Mapping;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.Mapping
{
    public class AdministratorMap : ClassMap<Administrator>
    {
        public AdministratorMap()
        {
            Id(x => x.Username)
                .Access.CamelCaseField(Prefix.Underscore)
                .GeneratedBy.Assigned();
        }
    }
}
