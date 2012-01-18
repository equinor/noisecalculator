//using System.Collections.Generic;
//using NUnit.Framework;
//using NoiseCalculator.Domain.DomainServices;
//using NoiseCalculator.Domain.Entities;

//namespace NoiseCalculator.Domain.Tests.DomainServices
//{
//    [TestFixture]
//    public class TaskCalculatorTests
//    {
//        //[MethodName_StateUnderTest_ExpectedBehavior]
//        [Test]
//        public void CalculateTotalDailyNoiseDosage_OneTaskWith25PercentOfDailyNoiseDosage_Calculated25Percent()
//        {
//            // Arrange
//            IList<Task> tasks = new List<Task>();
//            tasks.Add(new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109, ActualExposure = 90 }); // Equals to 25% daily dosage

//            // Act
//            ITaskCalculator taskCalculator = new TaskCalculator();
//            int totalPercentage = taskCalculator.CalculateTotalDailyNoiseDosage(tasks);

//            // Assert
//            Assert.AreEqual(25, totalPercentage);
//        }

//        [Test]
//        public void CalculateTotalDailyNoiseDosage_ThreeTasksWithATotalOf75PercentOfDailyNoiseDosage_Calculated75Percent()
//        {
//            // Arrange
//            IList<Task> tasks = new List<Task>();
//            tasks.Add(new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109, ActualExposure = 90 }); // Equals to 25% daily dosage
//            tasks.Add(new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109, ActualExposure = 90 }); // Equals to 25% daily dosage
//            tasks.Add(new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109, ActualExposure = 90 }); // Equals to 25% daily dosage

//            // Act
//            ITaskCalculator taskCalculator = new TaskCalculator();
//            int totalPercentage = taskCalculator.CalculateTotalDailyNoiseDosage(tasks);

//            // Assert
//            Assert.AreEqual(75, totalPercentage);
//        }
//    }
//}
