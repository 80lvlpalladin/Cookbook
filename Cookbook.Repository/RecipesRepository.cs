using Cookbook.Repository.DbContexts;
using Cookbook.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
[assembly:InternalsVisibleTo("Cookbook.Repository.Tests")]

namespace Cookbook.Repository
{
    public class RecipesRepository : IRecipesRepository
    {
        /// <summary>Constructor</summary>
        /// <param name="repoLocation">Location of repository db file. Root folder by default</param>
        public RecipesRepository(string repoLocation = null) =>
            _context = new RecipesContext(repoLocation);

        ///<inheritdoc/>
        public IEnumerable<RecipeLogEntry> GetLogEntries(int recipeId)
        {
            if (recipeId <= 0)
                throw new ArgumentOutOfRangeException("Recipe ID cannot be equal to zero or less.");

            CheckIfExists(recipeId);

            return _context.RecipesHistory.Where(entry => entry.RecipeID == recipeId)?
                .OrderBy(entry => entry.LastUpdated).ToArray();
        }

        ///<inheritdoc/>
        public Dictionary<RecipeNode, RecipeLogEntry> GetRecipes(int parentId = 0)
        {
            if (parentId < 0)
                throw new ArgumentOutOfRangeException("Recipe ID cannot be less than zero.");

            if (parentId != 0)
                CheckIfExists(parentId);

            string parentAncestryPath =
                _context.RecipesTree.FirstOrDefault(node => node.RecipeID == parentId)?.AncestryPath;

            IEnumerable<RecipeNode> recipeNodes;

            //this query fetches only the immediate children of a recipe
            if (parentAncestryPath is null && parentId == 0)
                recipeNodes = _context.RecipesTree.FromSqlRaw(
                    "SELECT * FROM RecipesTree " +
                    "WHERE AncestryPath LIKE '%/'" +
                    "AND AncestryPath NOT LIKE '%/%/'" +
                    "ORDER BY RecipeID");
            else if (parentAncestryPath is null && parentId != 0)
                return null;
            else
                recipeNodes = _context.RecipesTree.FromSqlRaw(
                    $"SELECT * FROM RecipesTree " +
                    $"WHERE AncestryPath LIKE '{parentAncestryPath}%' " +
                    $"AND AncestryPath NOT LIKE '{parentAncestryPath}%/%/' " +
                    $"AND AncestryPath <> '{parentAncestryPath}' " +
                    $"ORDER BY RecipeID");

            //this query fetches last log entries for abovementioned recipe nodes
            IEnumerable<RecipeLogEntry> recipeLogs = recipeNodes.Select(node =>
                _context.RecipesHistory.Where(entry => entry.RecipeID == node.RecipeID)
                .OrderByDescending(entry => entry.LastUpdated).First());

            return recipeNodes
                .Zip(recipeLogs, (node, entry) => new { node, entry })
                .ToDictionary(result => result.node, result => result.entry);
        }

        /// <summary>Adds new RecipeLogEntry object to RecipeHistory table</summary>
        /// <param name="created">If no created date is supplied, DateTime.Now is taken</param>
        /// <returns>If operation was successful</returns>
        private bool AddNewLogEntry(string title, string description, int recipeId, DateTime? created = null)
        {
            var newVersionId = (_context.RecipesHistory.DefaultIfEmpty()?.Max(entry => entry.VersionID) ?? 0) + 1;

            var latestLogEntry = GetLatestLogEntryFor(recipeId);
            if (latestLogEntry?.Title == title && latestLogEntry?.Description == description)
                throw new ArgumentException("Latest log entry for this recipe is exactly the same");

            _context.RecipesHistory.Add(
                new RecipeLogEntry()
                {
                    VersionID = newVersionId,
                    RecipeID = recipeId,
                    LastUpdated = created ?? DateTime.Now,
                    Description = description,
                    Title = title
                });

            return _context.SaveChanges() > 0;
        }

        ///<inheritdoc/>
        public int AddRecipe(string title, string description, string parentAncestryPath = null)
        {
            if (parentAncestryPath != null)
                CheckIfPathIsValid(parentAncestryPath);
            var newId = (_context.RecipesTree.DefaultIfEmpty()?.Max(node => node.RecipeID) ?? 0) + 1;
            var newPath = parentAncestryPath + newId + '/';
            var created = DateTime.Now;
            _context.RecipesTree.Add(
                new RecipeNode() { RecipeID = newId, AncestryPath = newPath, Created = created });
            if (AddNewLogEntry(title, description, newId, created))
                return newId;
            else return 0;
        }

        ///<inheritdoc/>
        public bool UpdateRecipe(string title, string description, int recipeId)
        {
            if (_context.RecipesTree.FirstOrDefault(node => node.RecipeID == recipeId) is null)
                throw new InvalidOperationException($"Recipe with id '{recipeId}' does not exist");

            return AddNewLogEntry(title, description, recipeId);
        }

        ///<inheritdoc/>
        public void Dispose() => _context.Dispose();

        internal RecipeLogEntry GetLatestLogEntryFor(int recipeId)
        {
            return _context.RecipesHistory.Where(entry => entry.RecipeID == recipeId)
                .OrderByDescending(entry => entry.LastUpdated).FirstOrDefault();
        }

        internal RecipeNode GetRecipeNode(int recipeId)
        {
            CheckIfExists(recipeId);
            return _context.RecipesTree.First(node => node.RecipeID == recipeId);
        }

        private void CheckIfPathIsValid(string ancestryPath)
        {
            if (_context.RecipesTree.FirstOrDefault(node => node.AncestryPath == ancestryPath) is null)
                throw new InvalidOperationException($"No recipe with ancestry path '{ancestryPath}' found.");
        }

        private void CheckIfExists(int recipeId)
        {
            if (_context.RecipesTree.Find(recipeId) is null ||
                _context.RecipesHistory.FirstOrDefault(entry => entry.RecipeID == recipeId) is null)
                throw new InvalidOperationException($"No recipe with id '{recipeId}' found.");
        }

        private readonly RecipesContext _context;
    }
}
