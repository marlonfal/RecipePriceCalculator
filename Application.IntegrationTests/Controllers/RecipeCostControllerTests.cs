using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.RecipeCost;
using Application.RecipeCost.Dto;
using Castle.Components.DictionaryAdapter;
using Domain.Entities;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Moq;
using NUnit.Framework;
using WebUI.Controllers;

namespace Application.IntegrationTests.Controllers
{
    public class RecipeCostControllerTests
    {
        private readonly Mock<IRecipeCostService> _recipeCostServiceMock = new Mock<IRecipeCostService>();
        private RecipeCostController GetController() => new RecipeCostController(_recipeCostServiceMock.Object);
        private readonly Random _random = new Random();


        [Test]
        public async Task Get_WithExistingRecipes_ReturnsRecipeList()
        {
            // Arrange
            var expectedResult = CreateRandomRecipeCostList();

            _recipeCostServiceMock
                .Setup(x => x.Get())
                .ReturnsAsync(expectedResult);

            var controller = GetController();

            // Act
            var result = await controller.Get();

            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var list = okObjectResult.Value as List<RecipeCostDto>;
            Assert.NotNull(list);
            Assert.True(list.Count > 0);
            Assert.AreEqual(expectedResult, list);
        }

        [Test]
        public async Task Get_WithNotExistingRecipes_ReturnsEmptyList()
        {
            // Arrange
            _recipeCostServiceMock
                .Setup(x => x.Get())
                .ReturnsAsync(new List<RecipeCostDto>());

            var controller = GetController();

            // Act
            var result = await controller.Get();

            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var list = okObjectResult.Value as List<RecipeCostDto>;
            Assert.NotNull(list);
            Assert.False(list.Count > 0);
        }

        [Test]
        public async Task Get_WithServiceError_ReturnsStatusCode500()
        {
            // Arrange
            _recipeCostServiceMock
                .Setup(x => x.Get())
                .ReturnsAsync((List<RecipeCostDto>)null);

            var controller = GetController();

            // Act
            var result = await controller.Get();

            // Assert
            var statusCodeResult  = result as StatusCodeResult;
            Assert.NotNull(statusCodeResult);
            Assert.AreEqual(statusCodeResult.StatusCode, 500);
        }

        [Test]
        public async Task Get_WithExistingRecipe_ReturnsExpectedRecipe()
        {
            // Arrange
            var expectedResult = CreateRandomRecipeCost();

            _recipeCostServiceMock
                .Setup(x => x.Get(It.IsAny<int>()))
                .ReturnsAsync(expectedResult);

            var controller = GetController();

            // Act 
            var result = await controller.Get(expectedResult.Recipe.RecipeId);

            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var model = okObjectResult.Value as RecipeCostDto;
            Assert.NotNull(model);
            Assert.AreEqual(expectedResult, model);
        }

        [Test]
        public async Task Get_WithNotExistingRecipe_ReturnsNotFound()
        {
            // Arrange
            var expectedResult = CreateRandomRecipeCost();

            _recipeCostServiceMock
                .Setup(x => x.Get(It.IsAny<int>()))
                .ReturnsAsync((RecipeCostDto)null);

            var controller = GetController();

            // Act 
            var result = await controller.Get(expectedResult.Recipe.RecipeId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }


        public List<RecipeCostDto> CreateRandomRecipeCostList()
        {
            return new List<RecipeCostDto>()
            {
                CreateRandomRecipeCost(),
                CreateRandomRecipeCost(),
                CreateRandomRecipeCost(),
            };
        }

        public RecipeCostDto CreateRandomRecipeCost()
        {
            return new RecipeCostDto()
            {
                Recipe = new Recipe()
                {
                    RecipeId = _random.Next(),
                    Name = Guid.NewGuid().ToString()
                }
            };
        }
    }
}
