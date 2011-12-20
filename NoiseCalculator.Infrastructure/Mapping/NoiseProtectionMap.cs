using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        }
    }
}
