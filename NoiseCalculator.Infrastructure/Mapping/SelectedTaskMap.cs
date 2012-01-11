using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.Mapping
{
    public class SelectedTaskMap : ClassMap<SelectedTask>
    {
        public SelectedTaskMap()
        {
            Id(x => x.Id);

            Map(x => x.Title);
            Map(x => x.Role);
            Map(x => x.NoiseProtection);
            Map(x => x.NoiseLevel);
            Map(x => x.Hours);
            Map(x => x.Minutes);
            Map(x => x.Percentage);

            Map(x => x.TaskId);
            Map(x => x.HelicopterTaskId);
        }
    }
}
