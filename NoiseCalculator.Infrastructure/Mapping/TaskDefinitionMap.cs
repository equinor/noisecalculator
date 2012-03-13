﻿using FluentNHibernate.Mapping;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.Mapping
{
    public class TaskDefinitionMap : ClassMap<TaskDefinition>
    {
        public TaskDefinitionMap()
        {
            Id(x => x.Id);

            Map(x => x.SystemName);
        }
    }
}
