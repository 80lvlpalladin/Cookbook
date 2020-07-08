using System;

namespace Cookbook.API.Models
{
    /// <summary>Recipe data transfer object that is going to be transmitted via http </summary>
    public class RecipeDto
    {
        /// <summary>Unique identifier of the recipe</summary>
        public int RecipeID { get; set; }

        /// <summary>
        /// Unique identifier of the position of the recipe in recipe tree.
        /// By default, if no ancestry type is specified, Recipe is placed on the top level of the tree
        /// </summary>
        public string AncestryPath { get; set; }

        /// <summary>Date recipe was last updated</summary>
        public DateTime Created { get; set; }

        /// <summary>Recipe title</summary>
        public string Title { get; set; }

        /// <summary>Recipe description</summary>
        public string Description { get; set; }
    }
}
