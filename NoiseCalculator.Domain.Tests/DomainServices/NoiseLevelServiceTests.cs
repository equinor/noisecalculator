using System.Collections.Generic;
using NUnit.Framework;
using NoiseCalculator.Domain.DomainServices;
using NoiseCalculator.Domain.Entities;

namespace NoiseCalculator.Domain.Tests.DomainServices
{
    [TestFixture]
    public class TaskCalculatorTests
    {
        //[MethodName_StateUnderTest_ExpectedBehavior]
        [Test]
        public void CalculateNoiseLevelEnum_PercentageIs60_ReturnsEnumValueNormal()
        {
            // Arrange
            INoiseLevelService noiseLevelService = new NoiseLevelService();
            const int percentageDailyDosage = 60;

            // Act
            NoiseLevelEnum noiseLevelEnum = noiseLevelService.CalculateNoiseLevelEnum(percentageDailyDosage);

            // Assert
            Assert.IsTrue(noiseLevelEnum == NoiseLevelEnum.Normal);
        }

        [Test]
        public void CalculateNoiseLevelEnum_PercentageIs80_ReturnsEnumValueWarning()
        {
            // Arrange
            INoiseLevelService noiseLevelService = new NoiseLevelService();
            const int percentageDailyDosage = 80;

            // Act
            NoiseLevelEnum noiseLevelEnum = noiseLevelService.CalculateNoiseLevelEnum(percentageDailyDosage);

            // Assert
            Assert.IsTrue(noiseLevelEnum == NoiseLevelEnum.Warning);
        }

        [Test]
        public void CalculateNoiseLevelEnum_PercentageIsMoreThan100_ReturnsEnumValueCritical()
        {
            // Arrange
            INoiseLevelService noiseLevelService = new NoiseLevelService();
            const int percentageDailyDosage = 101;

            // Act
            NoiseLevelEnum noiseLevelEnum = noiseLevelService.CalculateNoiseLevelEnum(percentageDailyDosage);

            // Assert
            Assert.IsTrue(noiseLevelEnum == NoiseLevelEnum.Critical);
        }

        [Test]
        public void CalculateNoiseLevelEnum_PercentageIsExactly100_ReturnsEnumValueMaximumAllowedDosage()
        {
            // Arrange
            INoiseLevelService noiseLevelService = new NoiseLevelService();
            const int percentageDailyDosage = 100;

            // Act
            NoiseLevelEnum noiseLevelEnum = noiseLevelService.CalculateNoiseLevelEnum(percentageDailyDosage);

            // Assert
            Assert.IsTrue(noiseLevelEnum == NoiseLevelEnum.MaximumAllowedDosage);
        }
    }
}
