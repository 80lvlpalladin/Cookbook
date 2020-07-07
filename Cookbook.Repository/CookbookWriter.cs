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

        public bool AddRecipe(string title, string description, string parentAncestryPath = null)
        {
            var newId = _context.RecipesTree.Max(node => node.RecipeID) + 1;
            var newPath = parentAncestryPath + newId + '/';
            _context.RecipesTree.Add(new RecipeNode() { RecipeID = newId, AncestryPath = newPath });

            var newVersionId = _context.RecipesHistory.Max(entry => entry.VersionID) + 1;
            _context.RecipesHistory.Add(
                new RecipeLogEntry()
                {
                    VersionID = newVersionId,
                    RecipeID = newId,
                    LastUpdated = DateTime.Now,
                    Description = description,
                    Title = title
                });

            return _context.SaveChanges() > 0;
        }

    }
}
