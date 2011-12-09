using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoiseCalculator.Entities
{
    public class HelicopterNoiseDosage
    {
        public virtual int Id { get; private set; }
        public virtual HelicopterType HelicopterType { get; set; }
        public virtual HelicopterNoiseProtection HelicopterNoiseProtection { get; set; }
        public virtual HelicopterWorkInterval HelicopterWorkInterval { get; set; }
    }
}
