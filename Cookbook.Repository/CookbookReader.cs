using Cookbook.Repository.DbContexts;
using Cookbook.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cookbook.Repository
{
    /// <summary>Class responsible for Read operations on Cookbook repository</summary>
    public class CookbookReader : IDisposable
    {
        public CookbookReader() => 
            _context = new CookbookContext();

        ///<inheritdoc/>
        public void Dispose() => _context.Dispose();

        public IEnumerable<RecipeLogEntry> GetLogEntries(int recipeId)
        {
            return _context.RecipesHistory.Where(entry => entry.RecipeID == recipeId)?
                .OrderBy(entry => entry.LastUpdated).ToArray();
        }

        /// <summary>Get current versions of children of recipe with given ID</summary>
        /// <param name="parentId">0 if you want to get all root recipes</param>
        /// <returns>Dictionary of RecipeNode and RecipeLogEntry objects, null if no recipe with such id exists</returns>
        /// TODO: TEST THIS
        public Dictionary<RecipeNode, RecipeLogEntry> GetRecipes(int parentId = 0)
        {
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

        private readonly CookbookContext _context;
    }
}
