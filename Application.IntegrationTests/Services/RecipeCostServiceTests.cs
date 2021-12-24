using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Parameters;
using Application.RecipeCost;
using Application.RecipeCost.Dto;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Application.IntegrationTests.Services
{
    using static  Testing;
    public class RecipeCostServiceTests
    {
        private readonly Mock<ILogger<RecipeCostService>> _loggerMock = new Mock<ILogger<RecipeCostService>>();
        private readonly Mock<IParameterService> _parameterServiceMock = new Mock<IParameterService>();

        private readonly RecipeCostService _service;

        public RecipeCostServiceTests()
        {
            _parameterServiceMock
                .Setup(x => x.Get_Decimal(Domain.Enums.Parameters.WellnessDiscount))
                .Returns(5M);

            _parameterServiceMock
                .Setup(x => x.Get_Decimal(Domain.Enums.Parameters.SaleTax))
                .Returns(8.6M);


            _service = new RecipeCostService(GetApplicationDbContext(), _loggerMock.Object,
                _parameterServiceMock.Object);
        }

        [Test]
        public async Task Get_WithExistingRecipes_ReturnsRecipesCost()
        {
            // Arrange
            var expectedAmount = GetApplicationDbContext().Recipes.Count();

            // Act
            var result  = await _service.Get();

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.Count, expectedAmount);
        }
        
        [Test]
        public async Task Get_WithExistingRecipe1_ReturnsRecipeCost()
        {
            // Arrange


            // Act
            var result  = await _service.Get(1);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.SaleTax, 0.21M);
            Assert.AreEqual(result.WellnessDiscount, 0.11M);
            Assert.AreEqual(result.Total, 4.45M);
        }
        
        [Test]
        public async Task Get_WithExistingRecipe2_ReturnsRecipeCost()
        {
            // Arrange


            // Act
            var result  = await _service.Get(2);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.SaleTax, 0.91M);
            Assert.AreEqual(result.WellnessDiscount, 0.09M);
            Assert.AreEqual(result.Total, 11.84M);
        }
        
        [Test]
        public async Task Get_WithNotExistingRecipe_ReturnsNull()
        {
            // Arrange


            // Act
            var result  = await _service.Get(5);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public void GetProductPrice_WithQuantityFourAndPriceHalfDollar_ReturnsPrice()
        {
            // Arrange
            var expectedResult = 4M * 0.5M;

            // Act
            var result = _service.GetProductPrice(4M, 0.5M);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetProductTax_WithFourDollarProduct_ReturnsTax()
        {
            // Arrange
            var expectedResult = 4M *_service.GetSaleTaxPercentage();

            // Act
            var result = _service.GetProductTax(4M);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetWellnessDiscount_WithFourDollarProduct_ReturnsDiscount()
        {
            // Arrange
            var expectedResult = 4M *_service.GetWellnessDiscountPercentage();

            // Act
            var result = _service.GetProductWellnessDiscount(4M);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
