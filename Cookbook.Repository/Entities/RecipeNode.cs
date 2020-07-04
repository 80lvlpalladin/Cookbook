namespace Cookbook.Repository.Entities
{
    /// <summary>Entity class reflecting recipe position in recipes tree</summary>
    public class RecipeNode
    {
        /// <summary>Unique identifier of the recipe</summary>
        public int RecipeID { get; set; }

        /// <summary>
        /// Unique identifier of the position of the recipe in recipe tree.
        /// By default, if no ancestry type is specified, Recipe is placed on the top level of the tree
        /// </summary>
        public string AncestryPath { get; set; }
    }
}
