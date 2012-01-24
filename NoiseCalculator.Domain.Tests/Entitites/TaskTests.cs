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
        public void CalculatePercentage_AllowedExposureIs360AndNoiseLevelIs109WithActualTime90_25Percent()
        {
            // Arrange
            Task task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109 };
            TimeSpan actualExposure = new TimeSpan(0, 0, 90, 0);
            
            // Act
            decimal calculatedPercentage = task.CalculatePercentage(task.NoiseLevelGuideline, actualExposure);

            // Assert
            Assert.AreEqual(25, calculatedPercentage);
        }

        [Test]
        public void CalculatePercentage_AllowedExposureIs360AndNoiseLevelIs109WithActualTime90AndNoiseLevelMeassuredIs112_50Percent()
        {
            // Arrange
            Task task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109 };
            TimeSpan actualExposure = new TimeSpan(0, 0, 90, 0);
            const int actualNoiseLevel = 112;

            // Act
            decimal calculatedPercentage = task.CalculatePercentage(actualNoiseLevel, actualExposure);

            // Assert
            Assert.AreEqual(50, calculatedPercentage);
        }

        [Test]
        public void CalculatePercentage_AllowedExposureIs360AndNoiseLevelIs109WithActualTime90AndNoiseLevelMeassuredIs113_62point5Percent()
        {
            // Arrange
            Task task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109 };
            TimeSpan actualExposure = new TimeSpan(0, 0, 90, 0);
            const int actualNoiseLevel = 113;

            // Act
            decimal calculatedPercentage = task.CalculatePercentage(actualNoiseLevel, actualExposure);

            // Assert
            Assert.AreEqual(62.5, calculatedPercentage);
        }

        [Test]
        public void CalculatePercentage_AllowedExposureIs360AndNoiseLevelIs109WithActualTime90AndNoiseLevelMeassuredIs114_83point3Percent()
        {
            // Arrange
            Task task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109 };
            TimeSpan actualExposure = new TimeSpan(0, 0, 90, 0);
            const int actualNoiseLevel = 114;

            // Act
            decimal calculatedPercentage = task.CalculatePercentage(actualNoiseLevel, actualExposure);

            // Assert
            Assert.AreEqual(83.3, Math.Round(calculatedPercentage, 1));
        }

        [Test]
        public void CalculatePercentage_AllowedExposureIs360AndNoiseLevelIs109WithActualTime90AndNoiseLevelMeassuredIs115_100Percent()
        {
            // Arrange
            Task task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109 };
            TimeSpan actualExposure = new TimeSpan(0, 0, 90, 0);
            const int actualNoiseLevel = 115;

            // Act
            decimal calculatedPercentage = task.CalculatePercentage(actualNoiseLevel, actualExposure);

            // Assert
            Assert.AreEqual(100, calculatedPercentage);
        }

        [Test]
        public void CalculatePercentage_AllowedExposureIs360AndNoiseLevelIs109WithActualTime180_50Percent()
        {
            // Arrange
            Task task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109 };
            TimeSpan actualExposure = new TimeSpan(0, 0, 180, 0);

            // Act
            decimal calculatedPercentage = task.CalculatePercentage(task.NoiseLevelGuideline, actualExposure);

            // Assert
            Assert.AreEqual(50, calculatedPercentage);
        }

        [Test]
        public void CalculateTimeSpan_AllowedExposureIs360AndNoiseLevelIs109WithPercent25_90Minutes()
        {
            // Arrange
            Task task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109 };
            const int actualNoiseLevel = 109;
            const int percentage = 25;

            // Act
            TimeSpan allowedTimeSpan = task.CalculateTimeSpan(actualNoiseLevel, percentage);

            // Assert
            Assert.That(allowedTimeSpan.TotalMinutes, Is.EqualTo(90));
        }

        [Test]
        public void CalculateTimeSpan_AllowedExposureIs360AndNoiseLevelIs109WithNoiseLevelMeasured112AndPercent25_90Minutes()
        {
            // Arrange
            Task task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109 };
            const int actualNoiseLevel = 112;
            const int percentage = 25;

            // Act
            TimeSpan allowedTimeSpan = task.CalculateTimeSpan(actualNoiseLevel, percentage);

            // Assert
            Assert.That(allowedTimeSpan.TotalMinutes, Is.EqualTo(45));
        }
    }
}
