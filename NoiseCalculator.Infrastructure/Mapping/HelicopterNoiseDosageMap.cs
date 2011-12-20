using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.Mapping
{
    public class HelicopterNoiseDosageMap : ClassMap<HelicopterNoiseDosage>
    {
        public HelicopterNoiseDosageMap()
        {
            Id(x => x.Id);

            Map(x => x.NoiseLevel);

            References(x => x.HelicopterType);
            References(x => x.HelicopterNoiseProtection);
            References(x => x.HelicopterWorkInterval);
        }
    }
}
