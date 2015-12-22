using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Domain.Tests.Entitites
{
    [TestClass]
    public class TaskTests
    {
        //[MethodName_StateUnderTest_ExpectedBehavior]

        [TestMethod]
        public void CalculatePercentage_AllowedExposureIs360AndNoiseLevelIs109WithActualTime90_25Percent()
        {
            // Arrange
            var task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109 };
            var actualExposure = new TimeSpan(0, 0, 90, 0);
            const int backgroundNoise = 0;
            
            // Act
            var calculatedPercentage = task.CalculatePercentage("", task.NoiseLevelGuideline, task.ButtonPressed, backgroundNoise, "", task.NoiseProtection, actualExposure);

            // Assert
            Assert.AreEqual(25, calculatedPercentage);
        }

        [TestMethod]
        public void CalculatePercentage_AllowedExposureIs360AndNoiseLevelIs109WithActualTime90AndNoiseLevelMeassuredIs112_50Percent()
        {
            // Arrange
            var task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109 };
            var actualExposure = new TimeSpan(0, 0, 90, 0);
            const int actualNoiseLevel = 112;
            const int backgroundNoise = 0;

            // Act
            var calculatedPercentage = task.CalculatePercentage("", actualNoiseLevel, task.ButtonPressed, backgroundNoise, "", task.NoiseProtection, actualExposure);

            // Assert
            Assert.AreEqual(50, calculatedPercentage);
        }

        [TestMethod]
        public void CalculatePercentage_AllowedExposureIs360AndNoiseLevelIs109WithActualTime90AndNoiseLevelMeassuredIs113_62point5Percent()
        {
            // Arrange
            var task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109 };
            var actualExposure = new TimeSpan(0, 0, 90, 0);
            const int actualNoiseLevel = 113;
            const int backgroundNoise = 0;

            // Act
            var calculatedPercentage = task.CalculatePercentage("", actualNoiseLevel, task.ButtonPressed, backgroundNoise, "", task.NoiseProtection, actualExposure);

            // Assert
            Assert.AreEqual(62.5m, calculatedPercentage);
        }

        [TestMethod]
        public void CalculatePercentage_AllowedExposureIs360AndNoiseLevelIs109WithActualTime90AndNoiseLevelMeassuredIs114_83point3Percent()
        {
            // Arrange
            var task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109 };
            var actualExposure = new TimeSpan(0, 0, 90, 0);
            const int actualNoiseLevel = 114;
            const int backgroundNoise = 0;

            // Act
            var calculatedPercentage = task.CalculatePercentage("", actualNoiseLevel, task.ButtonPressed, backgroundNoise, "", task.NoiseProtection, actualExposure);

            // Assert
            Assert.AreEqual(83.3m, Math.Round(calculatedPercentage, 1));
        }

        [TestMethod]
        public void CalculatePercentage_AllowedExposureIs360AndNoiseLevelIs109WithActualTime90AndNoiseLevelMeassuredIs115_100Percent()
        {
            // Arrange
            var task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109 };
            var actualExposure = new TimeSpan(0, 0, 90, 0);
            const int actualNoiseLevel = 115;
            const int backgroundNoise = 0;

            // Act
            var calculatedPercentage = task.CalculatePercentage("", actualNoiseLevel, task.ButtonPressed, backgroundNoise, "", task.NoiseProtection, actualExposure);

            // Assert
            Assert.AreEqual(100, calculatedPercentage);
        }

        [TestMethod]
        public void CalculatePercentage_AllowedExposureIs360AndNoiseLevelIs109WithActualTime180_50Percent()
        {
            // Arrange
            var task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109 };
            var actualExposure = new TimeSpan(0, 0, 180, 0);
            const int backgroundNoise = 0;

            // Act
            var calculatedPercentage = task.CalculatePercentage("",task.NoiseLevelGuideline, task.ButtonPressed, backgroundNoise, "", task.NoiseProtection, actualExposure);

            // Assert
            Assert.AreEqual(50, calculatedPercentage);
        }

        [TestMethod]
        public void CalculateTimeSpan_AllowedExposureIs360AndNoiseLevelIs109WithPercent25_90Minutes()
        {
            // Arrange
            var task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109 };
            const int actualNoiseLevel = 109;
            const int percentage = 25;
            const int buttonPressed = 100;
            const int backgroundNoise = 0;
            var noiseProtection = new NoiseProtection();
            
            // Act
            var allowedTimeSpan = task.CalculateTimeSpan("",actualNoiseLevel, buttonPressed, backgroundNoise, "", noiseProtection, percentage);

            // Assert
            Assert.AreEqual(90, allowedTimeSpan.TotalMinutes);
        }

        [TestMethod]
        public void CalculateTimeSpan_AllowedExposureIs360AndNoiseLevelIs109WithNoiseLevelMeasured112AndPercent25_90Minutes()
        {
            // Arrange
            var task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109 };
            const int actualNoiseLevel = 112;
            const int percentage = 25;
            const int buttonPressed = 100;
            const int backgroundNoise = 0;
            var noiseProtection = new NoiseProtection();

            // Act
            var allowedTimeSpan = task.CalculateTimeSpan("",actualNoiseLevel, buttonPressed, backgroundNoise, "", noiseProtection, percentage);

            // Assert
            Assert.AreEqual(45, allowedTimeSpan.TotalMinutes);
        }
    }
}
