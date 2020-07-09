using Cookbook.Repository.DbContexts;
using Cookbook.Repository.Entities;
using System;
using System.IO;

namespace Cookbook.Repository.Tests.TestUtils
{
    /// <summary>Class for shared context between tests</summary>
    public class RepositoryFixture : IDisposable
    {
        /// <summary>ReInitializes repository (needed for some tests)</summary>
        public void ResetRepository()
        {
            Dispose();
            InitializeRepository();
        }

        /// <summary>This code runs before all the tests run</summary>
        public RepositoryFixture()
        {
            InitializeRepository();
        }

        /// <summary>This code runs after all the tests run </summary>
        public void Dispose() => File.Delete(DbFileLocation);

        public string DbFileLocation =>
            AppDomain.CurrentDomain.BaseDirectory + "Test.db";

        private void SeedRootRecipies(RecipesContext context)
        {
            var rootRecipies = new[]
            {
                "Pasta Carbonara",
                "Mac & Cheese",
                "Baked Trout",
                "British Fries",
                "Mashed Potatoes",
                "New York Pizza",
                "Tortellini",
                "Classic Taco",
                "Salmon Pate",
                "Chicken Kyiv"
            };

            for (int i = 1; i < 11; i++)
            {
                var created = DateTime.Now;
                context.RecipesTree.Add(new RecipeNode()
                {
                    RecipeID = i,
                    AncestryPath = i + "/",
                    Created = created
                });
                context.RecipesHistory.Add(new RecipeLogEntry()
                {
                    VersionID = i,
                    RecipeID = i,
                    LastUpdated = created,
                    Title = rootRecipies[i - 1],
                    Description = "Description for " + rootRecipies[i - 1]
                });
            }
        }

        private void InitializeRepository()
        {
            //creates a temporal test db file
            using var context = new RecipesContext(DbFileLocation);
            SeedRootRecipies(context);
            context.RecipesHistory.Add(new RecipeLogEntry()
            {
                VersionID = 11,
                RecipeID = 10,
                LastUpdated = DateTime.Now,
                Title = "Chicken Kyiv version 2",
                Description = "New Chicken Kyiv description"
            });
            context.RecipesHistory.Add(new RecipeLogEntry()
            {
                VersionID = 12,
                RecipeID = 10,
                LastUpdated = DateTime.Now,
                Title = "Chicken Kyiv version 3",
                Description = "New new Chicken Kyiv description"
            });
            var created = DateTime.Now;
            context.RecipesTree.Add(new RecipeNode() { RecipeID = 11, AncestryPath = "3/11/", Created = created });
            context.RecipesHistory.Add(new RecipeLogEntry()
            {
                VersionID = 13,
                RecipeID = 11,
                LastUpdated = created,
                Title = "Baked trout stuffed with beans",
                Description = "Description of baked trout stuffed with beans"
            });
            context.RecipesHistory.Add(new RecipeLogEntry()
            {
                VersionID = 14,
                RecipeID = 11,
                LastUpdated = DateTime.Now,
                Title = "Baked trout stuffed with beans version 2",
                Description = "new Description of baked trout stuffed with beans"
            });
            context.SaveChanges();
        }
    }
}
