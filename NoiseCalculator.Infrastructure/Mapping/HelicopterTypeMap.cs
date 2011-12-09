﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NoiseCalculator.Entities;

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
