using Cookbook.Repository.Entities;
using Cookbook.Repository.Tests.TestUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Cookbook.Repository.Tests
{
    public class RecipesRepositoryTest : IClassFixture<RepositoryFixture>, IDisposable
    {
        private RecipesRepository _repo;
        private RepositoryFixture _fixture;

        /// <summary>This code runs before each test</summary>
        /// <param name="fixture">Used for accessing any shared context variables</param>
        public RecipesRepositoryTest(RepositoryFixture fixture)
        {
            _repo = new RecipesRepository(fixture.DbFileLocation);
            _fixture = fixture;
        }

        /// <summary>This code runs after each test</summary>
        public void Dispose() => _repo.Dispose();

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void GetLogEntries_ThrowsArgumentOutOfRangeException_IfRecipeIdIsZeroOrLess(int recipeId)
        {
            //Act-Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _repo.GetLogEntries(recipeId));
        }

        [Fact]
        public void GetLogEntries_ThrowsInvalidOperationException_IfNoRecipeWithSuchIdExists()
        {
            //Arrange
            var recipeId = 500;

            //Act-Assert
            Assert.Throws<InvalidOperationException>(() => _repo.GetLogEntries(recipeId));
        }

        [Fact]
        public void GetLogEntries_ReturnsAllLogEntries_ForChickenKyiv()
        {
            //Arrange
            var expectedResult = new[]
            {
                new RecipeLogEntry(){VersionID=10, RecipeID=10, Title="Chicken Kyiv", Description="Description for Chicken Kyiv"},
                new RecipeLogEntry(){VersionID=11, RecipeID=10, Title="Chicken Kyiv version 2", Description="New Chicken Kyiv description"},
                new RecipeLogEntry(){VersionID=12, RecipeID=10, Title="Chicken Kyiv version 3", Description="New new Chicken Kyiv description"},
            };
            var chickenKyivId = 10;

            //Act
            var actualResult = _repo.GetLogEntries(chickenKyivId);

            //Assert
            Assert.True(actualResult.SequenceEqual(expectedResult, new LogEntryEqualityComparer()));
        }

        [Fact]
        public void GetLogEntries_ReturnsAllLogEntries_ForPastaCarbonara()
        {
            //Arrange
            var expectedResult = new[]
            {
                new RecipeLogEntry(){VersionID=1, RecipeID=1, Title="Pasta Carbonara", Description="Description for Pasta Carbonara"},
            };
            var pastaCarbonaraId = 1;

            //Act
            var actualResult = _repo.GetLogEntries(pastaCarbonaraId);

            //Assert
            Assert.True(actualResult.SequenceEqual(expectedResult, new LogEntryEqualityComparer()));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        public void GetRecipes_ThrowsArgumentOutOfRangeException_IfParentIdIsLessThanZero(int parentId)
        {
            //Act-Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _repo.GetRecipes(parentId));
        }

        [Fact]
        public void GetRecipes_ReturnsAllLatestVersionsOfRootRecipes_IfParentIdIsZero()
        {
            //Arrange
            var parentId = 0;
            var expectedResultKeys = Enumerable.Range(1, 10)
                .Select(i => new RecipeNode() { RecipeID = i, AncestryPath = i + "/" });
            var expectedResultValues = new[]
            {
                 new RecipeLogEntry(){ VersionID = 1,  RecipeID = 1,  Title ="Pasta Carbonara",       Description = "Description for Pasta Carbonara"},
                 new RecipeLogEntry(){ VersionID = 2,  RecipeID = 2,  Title ="Mac & Cheese",          Description = "Description for Mac & Cheese"},
                 new RecipeLogEntry(){ VersionID = 3,  RecipeID = 3,  Title ="Baked Trout",           Description = "Description for Baked Trout"},
                 new RecipeLogEntry(){ VersionID = 4,  RecipeID = 4,  Title ="British Fries",         Description = "Description for British Fries"},
                 new RecipeLogEntry(){ VersionID = 5,  RecipeID = 5,  Title ="Mashed Potatoes",       Description = "Description for Mashed Potatoes"},
                 new RecipeLogEntry(){ VersionID = 6,  RecipeID = 6,  Title ="New York Pizza",        Description = "Description for New York Pizza"},
                 new RecipeLogEntry(){ VersionID = 7,  RecipeID = 7,  Title ="Tortellini",            Description = "Description for Tortellini"},
                 new RecipeLogEntry(){ VersionID = 8,  RecipeID = 8,  Title ="Classic Taco",          Description = "Description for Classic Taco"},
                 new RecipeLogEntry(){ VersionID = 9,  RecipeID = 9,  Title ="Salmon Pate",           Description = "Description for Salmon Pate"},
                 new RecipeLogEntry(){ VersionID = 12, RecipeID = 10, Title="Chicken Kyiv version 3", Description = "New new Chicken Kyiv description"}
            };

            //Act
            var actualResult = _repo.GetRecipes(parentId);

            //Assert
            Assert.True(expectedResultKeys.SequenceEqual(actualResult.Keys, new NodeEqualityComparer()));
            Assert.True(expectedResultValues.SequenceEqual(actualResult.Values, new LogEntryEqualityComparer()));
        }

        [Fact]
        public void GetRecipes_ReturnsLatestChildRecipeVersions_GivenValidParentId()
        {
            //Arrange
            var parentId = 3;
            var expectedResult = new Dictionary<RecipeNode, RecipeLogEntry>()
            {
                {
                    new RecipeNode(){RecipeID = 11, AncestryPath = "3/11/"},
                    new RecipeLogEntry(){VersionID = 14, RecipeID = 11,
                        Title = "Baked trout stuffed with beans version 2",
                        Description = "new Description of baked trout stuffed with beans" }
                }
            };

            //Act
            var actualResult = _repo.GetRecipes(parentId);

            //Assert
            Assert.True(expectedResult.Keys.SequenceEqual(actualResult.Keys, new NodeEqualityComparer()));
            Assert.True(expectedResult.Values.SequenceEqual(actualResult.Values, new LogEntryEqualityComparer()));
        }

        [Fact]
        public void GetRecipes_ThrowsInvalidOperationException_IfNoRecipeWithSuchIdExists()
        {
            //Arrange
            var recipeId = 500;

            //Act-Assert
            Assert.Throws<InvalidOperationException>(() => _repo.GetRecipes(recipeId));
        }

        [Fact]
        public void AddRecipe_AddsRecipeToRootLevelWithNewId_GivenNoParentAncestryPath()
        {
            //Arrange
            var title = "Completely new recipe";
            var description = "Completely new description";
            string parentAncestryPath = null;
            var expectedNode = new RecipeNode() { RecipeID = 12, AncestryPath = "12/" };
            var expectedLogEntry = new RecipeLogEntry()
            {
                VersionID = 15,
                RecipeID = 12,
                Title = title,
                Description = description
            };

            //Act
            var recipeId = _repo.AddRecipe(title, description, parentAncestryPath);

            var actualNode = _repo.GetRecipeNode(recipeId);
            var actualLogEntry = _repo.GetLatestLogEntryFor(recipeId);

            //Assert
            Assert.True(recipeId > 0);
            Assert.Equal(actualNode, expectedNode, new NodeEqualityComparer());
            Assert.Equal(actualLogEntry, expectedLogEntry, new LogEntryEqualityComparer());
            _fixture.ResetRepository();
        }

        [Fact]
        public void AddRecipe_AddsChildRecipeToParentRecipe_GivenParentId()
        {
            //Arrange
            var title = "Almost completely new recipe";
            var description = "Almost completely new description";
            var parentAncestryPath = "3/11/";
            var expectedNode = new RecipeNode() { RecipeID = 12, AncestryPath = "3/11/12/" } ;
            var expectedLogEntry = new RecipeLogEntry()
            {
                VersionID = 15,
                RecipeID = 12,
                Title = title,
                Description = description
            };

            //Act
            var recipeId = _repo.AddRecipe(title, description, parentAncestryPath);

            var actualNode = _repo.GetRecipeNode(recipeId);
            var actualLogEntry = _repo.GetLatestLogEntryFor(recipeId);

            //Assert
            Assert.True(recipeId > 0);
            Assert.Equal(actualNode, expectedNode, new NodeEqualityComparer());
            Assert.Equal(actualLogEntry, expectedLogEntry, new LogEntryEqualityComparer());
            _fixture.ResetRepository();
        }

        [Fact]
        public void AddRecipe_ThrowsInvalidOperationException_IfNoRecipeWithSuchPathExists()
        {
            //Arrange
            var invalidPath = "InvalidPath";

            //Act-Assert
            Assert.Throws<InvalidOperationException>(() => _repo.AddRecipe("Test", "Test", invalidPath));
        }

        [Fact]
        public void UpdateRecipe_ThrowsInvalidOperationException_IfNoRecipeWithSuchIdExists()
        {
            //Arrange
            var invalidRecipeId = 500;

            //Act-Assert
            Assert.Throws<InvalidOperationException>(() => _repo.UpdateRecipe("Test", "Test", invalidRecipeId));
        }

        [Fact]
        public void UpdateRecipe_ThrowsArgumentException_IfRecipeIsUpdatedTwiceWithSameData()
        {
            //Arrange
            var title = "Chicken Kyiv version 3";
            var description = "New new Chicken Kyiv description";
            var id = 10;

            //Act-Assert
            Assert.Throws<ArgumentException>(() => _repo.UpdateRecipe(title, description, id));

        }
    }
}
