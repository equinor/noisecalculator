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
        public void CalculatePercentage_NoiseLevelIs109WithActualTime90AndNoiseLevelMeassuredIs112_20Percent()
        {
            // Arrange
            var task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109 };
            var actualExposure = new TimeSpan(0, 0, 90, 0);
            const int actualNoiseLevel = 112;
            const int buttonPressed = 25;
            const int backgroundNoise = 70;
            var noiseProtection = new NoiseProtection() { NoiseDampening = 24 };

            // Act
            var calculatedPercentage = task.CalculatePercentage("", actualNoiseLevel, buttonPressed, backgroundNoise, "", noiseProtection, actualExposure);

            // Assert
            Assert.AreEqual(20, (int)Math.Round(calculatedPercentage));
        }

        [TestMethod]
        public void CalculatePercentage_NoiseLevelIs109WithActualTime90AndNoiseLevelMeassuredIs113_25Percent()
        {
            // Arrange
            var task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109 };
            var actualExposure = new TimeSpan(0, 0, 90, 0);
            const int actualNoiseLevel = 113;
            const int buttonPressed = 25;
            const int backgroundNoise = 70;
            var noiseProtection = new NoiseProtection() { NoiseDampening = 24 };

            // Act
            var calculatedPercentage = task.CalculatePercentage("", actualNoiseLevel, buttonPressed, backgroundNoise, "", noiseProtection, actualExposure);

            // Assert
            Assert.AreEqual(25, (int)Math.Round(calculatedPercentage));
        }

        [TestMethod]
        public void CalculatePercentage_NoiseLevelIs109WithActualTime90AndNoiseLevelMeassuredIs114_31Percent()
        {
            // Arrange
            var task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109 };
            var actualExposure = new TimeSpan(0, 0, 90, 0);
            const int actualNoiseLevel = 114;
            const int buttonPressed = 25;
            const int backgroundNoise = 70;
            var noiseProtection = new NoiseProtection() { NoiseDampening = 24 };

            // Act
            var calculatedPercentage = task.CalculatePercentage("", actualNoiseLevel, buttonPressed, backgroundNoise, "", noiseProtection, actualExposure);

            // Assert
            Assert.AreEqual(31, (int)Math.Round(calculatedPercentage));
        }

        [TestMethod]
        public void CalculatePercentage_NoiseLevelIs109WithActualTime90AndNoiseLevelMeassuredIs115_39Percent()
        {
            // Arrange
            var task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109 };
            var actualExposure = new TimeSpan(0, 0, 90, 0);
            const int actualNoiseLevel = 115;
            const int buttonPressed = 25;
            const int backgroundNoise = 70;
            var noiseProtection = new NoiseProtection() { NoiseDampening = 24 };

            // Act
            var calculatedPercentage = task.CalculatePercentage("", actualNoiseLevel, buttonPressed, backgroundNoise, "", noiseProtection, actualExposure);

            // Assert
            Assert.AreEqual(39, (int)Math.Round(calculatedPercentage));
        }

        [TestMethod]
        public void CalculatePercentage_NoiseLevelIs109WithActualTime180_20Percent()
        {
            // Arrange
            var task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109 };
            const int actualNoiseLevel = 109;
            const int buttonPressed = 25;
            var actualExposure = new TimeSpan(0, 0, 180, 0);
            const int backgroundNoise = 70;
            var noiseProtection = new NoiseProtection(){NoiseDampening = 24};

            // Act
            var calculatedPercentage = task.CalculatePercentage("", actualNoiseLevel, buttonPressed, backgroundNoise, "", noiseProtection, actualExposure);
            
            // Assert
            Assert.AreEqual(20, (int)Math.Round(calculatedPercentage));
        }

        [TestMethod]
        public void CalculateTimeSpan_NoiseLevelIs76WithPercent10_145Minutes()
        {
            // Arrange
            var task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109 };
            const int actualNoiseLevel = 76;
            const int percentage = 10;
            const int buttonPressed = 100;
            const int backgroundNoise = 70;
            var noiseProtection = new NoiseProtection();
            
            // Act
            var allowedTimeSpan = task.CalculateTimeSpan("",actualNoiseLevel, buttonPressed, backgroundNoise, "", noiseProtection, percentage);

            // Assert
            Assert.AreEqual(145, allowedTimeSpan.TotalMinutes);
        }

        [TestMethod]
        public void CalculateTimeSpan_NoiseLevelIs109WithNoiseLevelMeasured112AndPercent25AndButtonPressed25_114Minutes()
        {
            // Arrange
            var task = new Task { AllowedExposureMinutes = 360, NoiseLevelGuideline = 109 };
            const int actualNoiseLevel = 112;
            const int percentage = 25;
            const int buttonPressed = 25;
            const int backgroundNoise = 70;
            var noiseProtection = new NoiseProtection() {NoiseDampening = 24};

            // Act
            var allowedTimeSpan = task.CalculateTimeSpan("",actualNoiseLevel, buttonPressed, backgroundNoise, "", noiseProtection, percentage);

            // Assert
            Assert.AreEqual(114, allowedTimeSpan.TotalMinutes);
        }
    }
}
