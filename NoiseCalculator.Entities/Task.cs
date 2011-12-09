using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoiseCalculator.Entities
{
    public class Task
    {
        public virtual int Id { get; private set; }
        public virtual string Title { get; set; }
        public virtual Role Role { get; set; }
        public virtual NoiseProtection NoiseProtection { get; set; }
        public virtual int NoiseLevel { get; set; }
    }
}
