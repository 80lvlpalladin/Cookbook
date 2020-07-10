using Cookbook.API.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Cookbook.API.Tests.TestUtils
{
    /// <summary>Equaltity comparer for testing RecipeDto type for equality (ignores LastUpdated field)</summary>
    public class LogEntryDtoEqualityComparer : IEqualityComparer<RecipeLogEntryDto>
    {
        ///<inheritdoc/>
        public bool Equals([AllowNull] RecipeLogEntryDto x, [AllowNull] RecipeLogEntryDto y) =>
            x.Title == y.Title && x.Description == y.Description;

        public int GetHashCode([DisallowNull] RecipeLogEntryDto obj)
        {
            throw new NotImplementedException();
        }
    }
}
