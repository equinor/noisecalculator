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
            Map(x => x.CreatedBy).Not.Nullable();
            Map(x => x.CreatedDate).Not.Nullable();
            Map(x => x.IsNoiseMeassured).Not.Nullable();
            Map(x => x.ButtonPressed);
            Map(x => x.BackgroundNoise);
            Map(x => x.NoiseProtectionId);
            Map(x => x.UsePercentage);

            Map(x => x.HelicopterTaskId);

            References(x => x.Task, "TaskId").LazyLoad(Laziness.False);
        }
    }
}
