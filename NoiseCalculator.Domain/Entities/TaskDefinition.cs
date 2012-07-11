using System.Collections.Generic;
using NoiseCalculator.Domain.Enums;

namespace NoiseCalculator.Domain.Entities
{
    public class TaskDefinition
    {
        public virtual int Id { get; set; }
        public virtual string SystemName { get; set; }
        public virtual RoleTypeEnum RoleType { get; set; }
        public virtual IList<Task> Tasks { get; set; }
    }
}