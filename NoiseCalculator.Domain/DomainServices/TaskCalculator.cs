//using System;
//using System.Collections.Generic;
//using System.Linq;
//using NoiseCalculator.Domain.Entities;

//namespace NoiseCalculator.Domain.DomainServices
//{
//    public class TaskCalculator : ITaskCalculator
//    {
//        public int CalculateTotalDailyNoiseDosage(IEnumerable<Task> tasks)
//        {
//            decimal totalPercentage = tasks.Sum(x => x.CalculateDailyDosagePercentage());

//            return Convert.ToInt32(totalPercentage);
//        }
//    }
//}