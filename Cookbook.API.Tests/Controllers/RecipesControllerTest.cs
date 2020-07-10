using Cookbook.API.Controllers;
using Cookbook.API.Models;
using Cookbook.API.Tests.TestUtils;
using Cookbook.Repository;
using Cookbook.Repository.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Cookbook.API.Tests.Controllers
{
    /// <summary>Tests for RecipesController</summary>
    public class RecipesControllerTest
    {
        [Fact]
        public void GetRecipes_ReturnsJsonResultWithRecipeDtos_GivenValidParentId()
        {
            //Arrange
            var repoMock = new Mock<IRecipesRepository>();
            repoMock.Setup(repo => repo.GetRecipes(0))
                .Returns(new Dictionary<RecipeNode, RecipeLogEntry>()
                {
                    {
                        new RecipeNode(){ RecipeID = 1, AncestryPath = "1/"},
                        new RecipeLogEntry(){VersionID = 1, RecipeID = 1, Title = "Pizza", Description="Pizza description" }
                    },
                    {
                        new RecipeNode(){ RecipeID = 2, AncestryPath = "2/"},
                        new RecipeLogEntry(){VersionID = 2, RecipeID = 2, Title = "Cake", Description="Cake description" }
                    },
                    {
                        new RecipeNode(){ RecipeID = 3, AncestryPath = "3/"},
                        new RecipeLogEntry(){VersionID = 3, RecipeID = 3, Title = "Taco", Description="Taco description" }
                    }
                });
            var controller = new RecipesController(repoMock.Object);
            var expectedResult = new[]
            {
                new RecipeDto() {RecipeID = 1, AncestryPath = "1/", Title= "Pizza", Description = "Pizza description" },
                new RecipeDto() {RecipeID = 2, AncestryPath = "2/", Title= "Cake", Description = "Cake description" },
                new RecipeDto() {RecipeID = 3, AncestryPath = "3/", Title= "Taco", Description = "Taco description" }
            };

            //Act
            var result = controller.GetRecipes(0);

            //Assert
            var actualResult = Assert.IsType<JsonResult>(result);
            Assert.True(expectedResult.SequenceEqual((IEnumerable<RecipeDto>)(actualResult.Value), new RecipeDtoEqualityComparer()));
        }

        [Fact]
        public void GetRecipes_ReturnsNotFoundResult_IfNoSuchRecipeExists()
        {
            //Arrange
            var repoMock = new Mock<IRecipesRepository>(MockBehavior.Loose);
            var controller = new RecipesController(repoMock.Object);
            var invalidId = 8500;

            //Act
            var result = controller.GetRecipes(invalidId);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetRecipes_ReturnsBadRequestResult_IfRecipeIdIsLessThanZero()
        {
            //Arrange
            var repoMock = new Mock<IRecipesRepository>(MockBehavior.Loose);
            var controller = new RecipesController(repoMock.Object);
            var invalidId = -5;

            //Act
            var result = controller.GetRecipes(invalidId);

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void GetRecipeHistory_ReturnsBadRequestResult_IfRecipeIdIsLessThanZero()
        {
            //Arrange
            var repoMock = new Mock<IRecipesRepository>(MockBehavior.Loose);
            var controller = new RecipesController(repoMock.Object);
            var invalidId = -5;

            //Act
            var result = controller.GetRecipeHistory(invalidId);

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void GetRecipeHistory_ReturnsJsonResultWithLogEntryDtos_GivenValidRecipeId()
        {
            //Arrange
            var repoMock = new Mock<IRecipesRepository>();
            var validId = 5;
            repoMock.Setup(repo => repo.GetLogEntries(validId)).Returns(new[]
            {
                new RecipeLogEntry() { VersionID = 1, RecipeID = validId, Title = "Recipe", Description = "Description"},
                new RecipeLogEntry() { VersionID = 1, RecipeID = validId, Title = "Recipe version 1", Description = "Description 1"},
                new RecipeLogEntry() { VersionID = 1, RecipeID = validId, Title = "Recipe version 2", Description = "Description 2"}
            });
            var expectedResult = new[]
            {
                new RecipeLogEntryDto(){Title = "Recipe", Description = "Description"},
                new RecipeLogEntryDto(){Title = "Recipe version 1", Description = "Description 1"},
                new RecipeLogEntryDto(){Title = "Recipe version 2", Description = "Description 2"},
            };
            var controller = new RecipesController(repoMock.Object);

            //Act
            var result = controller.GetRecipeHistory(validId);

            //Assert
            var actualResult = Assert.IsType<JsonResult>(result);
            Assert.True(expectedResult.SequenceEqual((IEnumerable<RecipeLogEntryDto>)(actualResult.Value), new LogEntryDtoEqualityComparer()));

        }

        [Fact]
        public void UpdateRecipe_ReturnsOkResult_IfUpdateWasSuccessful()
        {
            //Arrange
            var repoMock = new Mock<IRecipesRepository>();
            repoMock.Setup(repo => repo.UpdateRecipe(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(true);
            var controller = new RecipesController(repoMock.Object);
            var updateRecipeDto = new UpdateRecipeDto();

            //Act
            var result = controller.UpdateRecipe(updateRecipeDto);

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void UpdateRecipe_ReturnsUnprocessableEntityResult_IfUpdateWasUnsuccessful()
        {
            //Arrange
            var repoMock = new Mock<IRecipesRepository>();
            repoMock.Setup(repo => repo.UpdateRecipe(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(false);
            var controller = new RecipesController(repoMock.Object);
            var updateRecipeDto = new UpdateRecipeDto();

            //Act
            var result = controller.UpdateRecipe(updateRecipeDto);

            //Assert
            Assert.IsType<UnprocessableEntityResult>(result);
        }

        [Fact]
        public void AddRecipe_ReturnsUnprocessableEntityResult_IfAddWasUnsuccessful()
        {
            //Arrange
            var repoMock = new Mock<IRecipesRepository>();
            repoMock.Setup(repo => repo.AddRecipe(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(0);
            var controller = new RecipesController(repoMock.Object);
            var newRecipeDto = new NewRecipeDto();

            //Act
            var result = controller.AddRecipe(newRecipeDto);

            //Assert
            Assert.IsType<UnprocessableEntityResult>(result);
        }

        [Fact]
        public void AddRecipe_ReturnsOkResult_IfUpdateWasSuccessful()
        {
            //Arrange
            var repoMock = new Mock<IRecipesRepository>();
            var newRecipeId = 5;
            repoMock.Setup(repo => repo.AddRecipe(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(newRecipeId);
            var controller = new RecipesController(repoMock.Object);
            var newRecipeDto = new NewRecipeDto();

            //Act
            var result = controller.AddRecipe(newRecipeDto);

            //Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
