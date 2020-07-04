using Cookbook.Repository.DbContexts;
using System;

namespace Cookbook.Repository
{
    /// <summary>Class responsible for Create, Update, Delete operations on Cookbook repository </summary>
    public class CookbookWriter : IDisposable
    {
        public CookbookWriter() => _context = new CookbookContext();

        ///<inheritdoc/>
        public void Dispose() => _context.Dispose();
       
        private readonly CookbookContext _context;

    }
}
