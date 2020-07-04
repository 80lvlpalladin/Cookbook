using System;

namespace Cookbook.Client.Models
{
    /// <summary>Client-side recipe class</summary>
    public class Recipe
    {
        /// <summary>Unique identifier of the recipe</summary>
        public int RecipeID { get; set; }

        /// <summary>
        /// Unique identifier of the position of the recipe in recipe tree.
        /// By default, if no ancestry type is specified, Recipe is placed on the top level of the tree
        /// </summary>
        public string AncestryPath { get; set; }

        /// <summary>Date recipe was last updated</summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>Recipe title</summary>
        public string Title { get; set; }

        /// <summary>Recipe description</summary>
        public string Description { get; set; }

        //public Recipe Child { get; set; }
    }
}
