using System;
using System.Collections.Generic;
using System.Text;

namespace Cookbook.Client.Models.DTOs
{
    public class UpdateRecipeDto
    {
        /// <summary>Unique identifier of the recipe</summary>
        public int RecipeID { get; set; }

        /// <summary>Recipe title</summary>
        public string Title { get; set; }

        /// <summary>Recipe description</summary>
        public string Description { get; set; }
    }
}
