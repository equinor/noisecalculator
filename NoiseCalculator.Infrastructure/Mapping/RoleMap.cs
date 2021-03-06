﻿using FluentNHibernate.Mapping;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.Mapping
{
    public class RoleMap : ClassMap<Role>
    {
        public RoleMap()
        {
            Id(x => x.Id);

            Map(x => x.Title);
            Map(x => x.CultureName);

            Map(x => x.RoleType);
            Map(x => x.SystemTitle);

            References(x => x.RoleDefinition).Not.Nullable();
        }
    }
}
