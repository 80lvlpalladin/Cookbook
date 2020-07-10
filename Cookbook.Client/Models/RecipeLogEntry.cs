using System;

namespace Cookbook.Client.Models
{
    /// <summary>
    /// Client-side class for RecipeLogEntry
    /// </summary>
    public class RecipeLogEntry
    {
        /// <summary>Date recipe was last updated</summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>Recipe title</summary>
        public string Title { get; set; }

        /// <summary>Recipe description</summary>
        public string Description { get; set; }
    }
}
