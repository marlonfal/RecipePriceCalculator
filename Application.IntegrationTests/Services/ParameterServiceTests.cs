using Application.Parameters;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Application.IntegrationTests.Services
{
    using static Testing;
    public class ParameterServiceTests
    {
        private readonly Mock<ILogger<ParameterService>> _loggerMock = new Mock<ILogger<ParameterService>>();

        private readonly ParameterService _parameterService;
        public ParameterServiceTests()
        {
            _parameterService = new ParameterService(GetApplicationDbContext(), _loggerMock.Object);
        }

        [Test]
        public void Get_WithExistingParameter_ReturnsExpectedParameter()
        {
            // Arrange


            // Act
            var result = _parameterService.Get(Domain.Enums.Parameters.WellnessDiscount);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.Key, Domain.Enums.Parameters.WellnessDiscount.ToString());
        }


        [Test]
        public void Get_Decimal_WithExistingParameter_ReturnsExpectedParameter()
        {
            // Arrange


            // Act
            var result = _parameterService.Get_Decimal(Domain.Enums.Parameters.WellnessDiscount);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(result, 5M);
        }

        [Test]
        public void Get_WithExistingParameter_ReturnsNull()
        {
            // Arrange


            // Act
            var result = _parameterService.Get(Domain.Enums.Parameters.TestFail);

            // Assert
            Assert.Null(result);
        }


        [Test]
        public void Get_Decimal_WithExistingParameter_ReturnsZero()
        {
            // Arrange


            // Act
            var result = _parameterService.Get_Decimal(Domain.Enums.Parameters.TestFail);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(result, 0M);
        }
    }
}
