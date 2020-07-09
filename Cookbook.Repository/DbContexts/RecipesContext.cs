using Cookbook.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Cookbook.Repository.Tests")]

namespace Cookbook.Repository.DbContexts
{
    /// <summary>DbContext class for the Cookbook</summary>
    internal class RecipesContext : DbContext
    {
        /// <summary>Creates a db file if absent</summary>
        internal RecipesContext(string dbfileUri = null)
        {
            _dbFileURI = dbfileUri ??
                AppDomain.CurrentDomain.BaseDirectory + "Cookbook.db";

            Database.EnsureCreated(); //https://github.com/dotnet/efcore/issues/21215
        }

        /// <summary>Table that reflects recipe tree structure</summary>
        internal DbSet<RecipeNode> RecipesTree { get; set; }

        /// <summary>Recipes history table</summary>
        internal DbSet<RecipeLogEntry> RecipesHistory { get; set; }
 
        ///<inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder builder) => 
            builder.UseSqlite("Data Source = " + _dbFileURI);

        ///<inheritdoc/>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<RecipeNode>().HasKey(recipe => recipe.RecipeID);
            builder.Entity<RecipeLogEntry>().HasKey(entry =>entry.VersionID);
            SeedInitialData(builder);
        }

        /// <summary>Database file location</summary>
        private readonly string _dbFileURI;

        private void SeedInitialData(ModelBuilder builder)
        {
            
        }       
    }
}
