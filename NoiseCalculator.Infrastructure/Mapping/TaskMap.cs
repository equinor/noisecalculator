using System.Threading;
using FluentNHibernate.Mapping;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.Mapping
{
    public class TaskMap : ClassMap<Task>
    {
        public TaskMap()
        {
            Id(x => x.Id);

            //Map(x => x.Title);
            Map(x => x.NoiseLevelGuideline);
            Map(x => x.AllowedExposureMinutes);
            Join("TaskTranslation",
                 x =>
                 {
                     x.KeyColumn("Task_id");
                     x.Map(m => m.Title); 
                 });
            
            //ApplyFilter("this_1_.CompanyId = :companyId")
            ApplyFilter<CultureNameFilterDefinition>();
            //ApplyFilter<CultureNameFilterDefinition>("CultureName = :companyId");
            //ApplyFilter<CultureNameFilterDefinition>("CultureName = :companyId"))

            References(x => x.Role);
            References(x => x.NoiseProtection);
        }
    }
}
