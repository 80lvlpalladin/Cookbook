using Cookbook.Repository.DbContexts;
using Cookbook.Repository.Entities;
using System;
using System.Linq;

namespace Cookbook.Repository
{
    /// <summary>Class responsible for Create, Update, Delete operations on Cookbook repository </summary>
    public class CookbookWriter : IDisposable
    {
        public CookbookWriter() => _context = new CookbookContext();

        ///<inheritdoc/>
        public void Dispose() => _context.Dispose();
       
        private readonly CookbookContext _context;

        private bool AddNewLogEntry(string title, string description, int recipeId, DateTime? created = null)
        {
            var newVersionId = _context.RecipesHistory.Max(entry => (int?)entry.VersionID) ?? 0 + 1;
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

        public bool AddRecipe(string title, string description, string parentAncestryPath = null)
        {
            var newId = _context.RecipesTree.Max(node => (int?)node.RecipeID) ?? 0 + 1;
            var newPath = parentAncestryPath + newId + '/';
            var created = DateTime.Now;
            _context.RecipesTree.Add(
                new RecipeNode() { RecipeID = newId, AncestryPath = newPath, Created = created});

            return AddNewLogEntry(title, description, newId, created);
        }

        public bool UpdateRecipe(string title, string description, int recipeId) 
        {
            if (_context.RecipesTree.FirstOrDefault(node => node.RecipeID == recipeId) is null)
                throw new InvalidOperationException($"Recipe with id '{recipeId}' does not exist");

            return AddNewLogEntry(title, description, recipeId);
        }
    }
}
