﻿using FluentNHibernate.Mapping;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.Mapping
{
    public class TaskMap : ClassMap<Task>
    {
        public TaskMap()
        {
            Id(x => x.Id);

            Map(x => x.Title);
            Map(x => x.NoiseLevelGuideline);
            Map(x => x.AllowedExposureMinutes);

            References(x => x.Role);
            References(x => x.NoiseProtection);
        }
    }
}
