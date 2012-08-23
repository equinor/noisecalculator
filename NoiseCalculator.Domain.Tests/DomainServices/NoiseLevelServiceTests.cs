using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoiseCalculator.Domain.DomainServices;
using NoiseCalculator.Domain.Enums;

namespace NoiseCalculator.Domain.Tests.DomainServices
{
    [TestClass]
    public class TaskCalculatorTests
    {
        //[MethodName_StateUnderTest_ExpectedBehavior]
        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void CalculateNoiseLevelEnum_PercentgeIs200_ReturnEnumValueWorkRelatedInjury()
        {
            // Arrange
            INoiseLevelService noiseLevelService = new NoiseLevelService();
            const int percentageDailyDosage = 200;

            // Act
            NoiseLevelEnum noiseLevelEnum = noiseLevelService.CalculateNoiseLevelEnum(percentageDailyDosage);

            // Assert
            Assert.IsTrue(noiseLevelEnum == NoiseLevelEnum.DangerOfWorkRelatedInjury);
        }

        [TestMethod]
        public void CalculateNoiseLevelEnum_PercentgeIsAbove200_ReturnEnumValueWorkRelatedInjury()
        {
            // Arrange
            INoiseLevelService noiseLevelService = new NoiseLevelService();
            const int percentageDailyDosage = 208;

            // Act
            NoiseLevelEnum noiseLevelEnum = noiseLevelService.CalculateNoiseLevelEnum(percentageDailyDosage);

            // Assert
            Assert.IsTrue(noiseLevelEnum == NoiseLevelEnum.DangerOfWorkRelatedInjury);
        }
    }
}
