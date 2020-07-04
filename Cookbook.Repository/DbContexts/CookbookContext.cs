using Cookbook.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Cookbook.Repository.DbContexts
{
    /// <summary>DbContext class for the Cookbook</summary>
    public class CookbookContext : DbContext
    {
        /// <summary>Creates a db file if absent</summary>
        public CookbookContext() => Database.EnsureCreated();

        /// <summary>Table that reflects recipe tree structure</summary>
        internal DbSet<RecipeNode> RecipesTree { get; set; }

        /// <summary>Recipes history table</summary>
        internal DbSet<RecipeLogEntry> RecipesHistory { get; set; }
 
        ///<inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder builder) => 
            builder.UseSqlite("Data Source = " + _dbFileURI.LocalPath);

        ///<inheritdoc/>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<RecipeNode>().HasKey(recipe => new { recipe.RecipeID, recipe.AncestryPath });
            builder.Entity<RecipeLogEntry>().HasKey(entry =>entry.VersionID);
            SeedInitialData(builder);
        }

        /// <summary>Database file location</summary>
        private readonly Uri _dbFileURI =
            new Uri(AppDomain.CurrentDomain.BaseDirectory + "Cookbook.db");

        private void SeedInitialData(ModelBuilder builder)
        {
            builder.Entity<RecipeNode>()
                .HasData(Enumerable.Range(1, 10).Select(i => new RecipeNode() { RecipeID = i, AncestryPath = $"{i}/" }));

            builder.Entity<RecipeNode>().HasData(Enumerable.Range(11, 3)
                .Select(i => new RecipeNode { RecipeID = i, AncestryPath = $"3/{i}" }));

            builder.Entity<RecipeLogEntry>().HasData(new RecipeLogEntry[]
            {               
                new RecipeLogEntry(){ VersionID = 1, RecipeID = 1, LastUpdated = DateTime.Now, Title = "Pasta Carbonara", Description = "Description for Pasta Carbonara"},
                new RecipeLogEntry(){ VersionID = 2, RecipeID = 2, LastUpdated = DateTime.Now, Title = "Mac & Cheese", Description = "Description for Mac & Cheese"},
                new RecipeLogEntry(){ VersionID = 3, RecipeID = 3, LastUpdated = DateTime.Now, Title = "Baked Trout", Description = "Description for Baked Trout"},
                new RecipeLogEntry(){ VersionID = 4, RecipeID = 4, LastUpdated = DateTime.Now, Title = "British Fries", Description = "Description for British Fries"},
                new RecipeLogEntry(){ VersionID = 5, RecipeID = 5, LastUpdated = DateTime.Now, Title = "Mashed Potatoes", Description = "Description for Mashed Potatoes"},
                new RecipeLogEntry(){ VersionID = 6, RecipeID = 6, LastUpdated = DateTime.Now, Title = "New York Pizza", Description = "Description for New York Pizza"},
                new RecipeLogEntry(){ VersionID = 7, RecipeID = 7, LastUpdated = DateTime.Now, Title = "Tortellini", Description = "Description for Tortellini"},
                new RecipeLogEntry(){ VersionID = 8, RecipeID = 8, LastUpdated = DateTime.Now, Title = "Classic Taco", Description = "Description for Classic Taco"},
                new RecipeLogEntry(){ VersionID = 9, RecipeID = 9, LastUpdated = DateTime.Now, Title = "Salmon Pate", Description = "Description for Salmon Pate"},
                new RecipeLogEntry(){ VersionID = 10,RecipeID = 10,LastUpdated = DateTime.Now, Title = "Chicken Kyiv", Description = "Description for Chicken Kyiv"},
                new RecipeLogEntry(){ VersionID = 11,RecipeID = 11,LastUpdated = DateTime.Now, Title = "Baked trout with apples", Description = "Description for Baked trout with apples"},
                new RecipeLogEntry(){ VersionID = 12,RecipeID = 12,LastUpdated = DateTime.Now, Title = "Baked trout with eggs", Description = "Description for Baked trout with eggs"},
                new RecipeLogEntry(){ VersionID = 13,RecipeID = 13,LastUpdated = DateTime.Now, Title = "Baked trout with oranges", Description = "Description for Baked trout with oranges"},

            });
        }
       
    }
}
