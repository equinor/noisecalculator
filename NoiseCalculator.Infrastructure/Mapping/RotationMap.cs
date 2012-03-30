using FluentNHibernate.Mapping;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Infrastructure.Mapping
{
    public class RotationMap : ClassMap<Rotation>
    {
        public RotationMap()
        {
            Id(x => x.Id);

            References(x => x.Task);
            References(x => x.OperatorTask).LazyLoad(Laziness.False);
            References(x => x.AssistantTask).LazyLoad(Laziness.False);
        }
    }
}
