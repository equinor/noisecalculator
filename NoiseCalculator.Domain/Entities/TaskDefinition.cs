using System.Collections.Generic;

namespace NoiseCalculator.Domain.Entities
{
    public class TaskDefinition
    {
        public virtual int Id { get; set; }
        public virtual string SystemName { get; set; }
        public IList<Task> Tasks { get; set; }
    }
}