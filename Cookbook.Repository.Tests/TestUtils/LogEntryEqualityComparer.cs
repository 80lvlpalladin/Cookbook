using Cookbook.Repository.Entities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Cookbook.Repository.Tests.TestUtils
{
    /// <summary>Equality comparer for testing LogEntries for equality (ignores LastUpdated field)</summary>
    public class LogEntryEqualityComparer : IEqualityComparer<RecipeLogEntry>
    {
        ///<inheritdoc/>
        public bool Equals([AllowNull] RecipeLogEntry x, [AllowNull] RecipeLogEntry y) =>
            x.Title == y.Title && x.Description == y.Description && x.RecipeID == y.RecipeID && x.VersionID == y.VersionID;

        ///<inheritdoc/>
        public int GetHashCode([DisallowNull] RecipeLogEntry obj) =>
                   obj.RecipeID.GetHashCode() ^
                   obj.Title.GetHashCode() ^
                   obj.Description.GetHashCode() ^
                   obj.VersionID.GetHashCode();
    }
}
