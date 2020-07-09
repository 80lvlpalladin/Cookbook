using System;
using System.Collections.Generic;
using System.Text;

namespace Cookbook.Client.Models
{
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
