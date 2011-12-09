﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NoiseCalculator.Entities;

namespace NoiseCalculator.Infrastructure.Mapping
{
    public class HelicopterWorkIntervalMap : ClassMap<HelicopterNoiseProtection>
    {
        public HelicopterWorkIntervalMap()
        {
            Id(x => x.Id);

            Map(x => x.Title);
        }
    }
}
