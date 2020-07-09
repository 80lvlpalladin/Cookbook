using Cookbook.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Cookbook.Repository.Tests.TestUtils
{
    /// <summary>Equality comparer for testing Recipe Nodes for equality (ignores Created field) </summary>
    public class NodeEqualityComparer : IEqualityComparer<RecipeNode>
    {
        ///<inheritdoc/>
        public bool Equals([AllowNull] RecipeNode x, [AllowNull] RecipeNode y) =>
            x.RecipeID == y.RecipeID && x.AncestryPath == y.AncestryPath;

        ///<inheritdoc/>
        public int GetHashCode([DisallowNull] RecipeNode obj) =>
            obj.AncestryPath.GetHashCode() ^ obj.RecipeID.GetHashCode();
    }
}
