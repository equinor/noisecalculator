using System;
using NUnit.Framework;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Domain.Tests.Entitites
{
    [TestFixture]
    public class TaskTests
    {
        //[MethodName_StateUnderTest_ExpectedBehavior]

        [Test]
        public void CalculateDailyDosagePercentage_AllowedExposureIs360AndNoiseLevelIs109WithActualTime90_25Percent()
        {
            // Arrange
            Task task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109 };
            TimeSpan actualExposure = new TimeSpan(0, 0, 90);
            
            // Act
            decimal calculatedPercentage = task.CalculateDailyDosagePercentage(task.NoiseLevelGuideline, actualExposure);

            // Assert
            Assert.AreEqual(25, calculatedPercentage);
        }

        [Test]
        public void CalculateDailyDosagePercentage_AllowedExposureIs360AndNoiseLevelIs109WithActualTime90AndNoiseLevelMeassuredIs112_50Percent()
        {
            // Arrange
            Task task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109, ActualExposure = 90 };
            const int actualNoiseLevel = 112;

            // Act
            decimal calculatedPercentage = task.CalculateDailyDosagePercentage(actualNoiseLevel);

            // Assert
            Assert.AreEqual(50, calculatedPercentage);
        }

        [Test]
        public void CalculateDailyDosagePercentage_AllowedExposureIs360AndNoiseLevelIs109WithActualTime90AndNoiseLevelMeassuredIs113_62point5Percent()
        {
            // Arrange
            Task task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109, ActualExposure = 90 };
            const int actualNoiseLevel = 113;

            // Act
            decimal calculatedPercentage = task.CalculateDailyDosagePercentage(actualNoiseLevel);

            // Assert
            Assert.AreEqual(62.5, calculatedPercentage);
        }

        [Test]
        public void CalculateDailyDosagePercentage_AllowedExposureIs360AndNoiseLevelIs109WithActualTime90AndNoiseLevelMeassuredIs114_83point3Percent()
        {
            // Arrange
            Task task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109, ActualExposure = 90 };
            const int actualNoiseLevel = 114;

            // Act
            decimal calculatedPercentage = task.CalculateDailyDosagePercentage(actualNoiseLevel);

            // Assert
            Assert.AreEqual(83.3, Math.Round(calculatedPercentage, 1));
        }

        [Test]
        public void CalculateDailyDosagePercentage_AllowedExposureIs360AndNoiseLevelIs109WithActualTime90AndNoiseLevelMeassuredIs115_100Percent()
        {
            // Arrange
            Task task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109, ActualExposure = 90 };
            const int actualNoiseLevel = 115;

            // Act
            decimal calculatedPercentage = task.CalculateDailyDosagePercentage(actualNoiseLevel);

            // Assert
            Assert.AreEqual(100, calculatedPercentage);
        }

        [Test]
        public void CalculateDailyDosagePercentage_AllowedExposureIs360AndNoiseLevelIs109WithActualTime180_50Percent()
        {
            // Arrange
            Task task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109, ActualExposure = 180 };

            // Act
            decimal calculatedPercentage = task.CalculateDailyDosagePercentage(task.NoiseLevelGuideline);

            // Assert
            Assert.AreEqual(50, calculatedPercentage);
        }
    }
}
