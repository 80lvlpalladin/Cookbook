using Cookbook.Repository.Entities;
using System;
using System.Collections.Generic;

namespace Cookbook.Repository
{
    /// <summary>Interface for recipes repository</summary>
    public interface IRecipesRepository : IDisposable
    {
        /// <summary>Get all log entries for the given recipe Id</summary>
        IEnumerable<RecipeLogEntry> GetLogEntries(int recipeId);

        /// <summary>Get current versions of children of recipe with given ID</summary>
        /// <param name="parentId">0 if you want to get all root recipes</param>
        /// <returns>Dictionary of RecipeNode and RecipeLogEntry objects, null if no recipe with such id exists</returns>
        Dictionary<RecipeNode, RecipeLogEntry> GetRecipes(int parentId = 0);

        /// <summary>Adds a new recipe to repository.</summary>
        /// <param name="parentAncestryPath">If no parent ancestry path is supplied, adds recipe to root level</param>
        /// <returns>Id of newly created recipe if operation was successful, otherwise - 0</returns>
        int AddRecipe(string title, string description, string parentAncestryPath = null);

        /// <summary>Updates existing recipe</summary>
        /// <returns>If operation was successful</returns>
        bool UpdateRecipe(string title, string description, int recipeId);
    }
}
