using System;

namespace Cookbook.Repository.Entities
{
    /// <summary>Enitity needed for logging change history of recipies</summary>
    public class RecipeLogEntry
    {
        /// <summary>Unique identifier of the recipe version</summary>
        public int VersionID { get; set; }

        /// <summary>Identifier of the recipe</summary>
        public int RecipeID { get; set; }

        /// <summary>Date recipe was last updated</summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>Recipe title</summary>
        public string Title { get; set; }

        /// <summary>Recipe description</summary>
        public string Description { get; set; }

    }
}
