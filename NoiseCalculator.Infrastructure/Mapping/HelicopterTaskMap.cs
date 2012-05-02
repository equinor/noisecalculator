using FluentNHibernate.Mapping;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.Mapping
{
    public class HelicopterTaskMap : ClassMap<HelicopterTask>
    {
        public HelicopterTaskMap()
        {
            Id(x => x.Id);

            Map(x => x.Percentage);

            References(x => x.HelicopterType);
            
            //References(x => x.HelicopterNoiseProtectionDefinition);
            References(x => x.HelicopterNoiseProtection); // <---- Nytt for test
            
            References(x => x.HelicopterWorkInterval);
            Map(x => x.CultureName); // <---- Nytt for test

            References(x => x.HelicopterTaskDefinition); // <---- Nytt for test
        }
    }
}
