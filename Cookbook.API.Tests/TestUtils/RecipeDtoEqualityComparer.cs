using Cookbook.API.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Cookbook.API.Tests.TestUtils
{
    /// <summary>
    /// Equaltity comparer for testing RecipeDto type for equality (ignores Created field)
    /// </summary>
    public class RecipeDtoEqualityComparer : IEqualityComparer<RecipeDto>
    {
        ///<inheritdoc/>
        public bool Equals([AllowNull] RecipeDto x, [AllowNull] RecipeDto y)
        {
            return x.Description == y.Description && x.RecipeID == y.RecipeID && 
                x.AncestryPath == y.AncestryPath && x.Title == y.Title;
        }

        public int GetHashCode([DisallowNull] RecipeDto obj)
        {
            throw new NotImplementedException();
        }
    }
}
