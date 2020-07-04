using Cookbook.Repository.DbContexts;
using Cookbook.Repository.Entities;
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

        /// <summary>Get current versions of all recipes</summary>
        /// <returns>Dictionary of RecipeNode and RecipeLogEntry objects</returns>
        public Dictionary<RecipeNode, RecipeLogEntry> GetRecipes()
        {
            IEnumerable<RecipeNode> recipeNodes = _context.RecipesTree.OrderBy(node => node.RecipeID);
            //IEnumerable<RecipeLogEntry> recipeLogs = _context.RecipesHistory.FromSqlRaw(
            //    "SELECT * FROM RecipesHistory as x " +
            //    "WHERE x.LastUpdated = (" +
            //        "SELECT MAX(LastUpdated) FROM RecipesHistory as y " +
            //        "WHERE y.RecipeID = x.RecipeID)");

            //this query gets the latest version of each recipe based on LastUpdated column
            IEnumerable<RecipeLogEntry> recipeLogs = _context.RecipesHistory
                .Where(entry => entry.LastUpdated == _context.RecipesHistory
                .Where(i => i.RecipeID == entry.RecipeID).Max(i => i.LastUpdated)).OrderBy(entry => entry.RecipeID);

            if (recipeLogs.Count() != recipeNodes.Count()) 
                throw new ArgumentOutOfRangeException("RecipeLogs and RecipeNodes lists must be equal");

            return recipeNodes
                .Zip(recipeLogs, (node, entry) => new { node, entry })
                .ToDictionary(result => result.node, result => result.entry);
        }

        private readonly CookbookContext _context;
    }
}
