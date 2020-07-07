using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cookbook.API.Models
{
    public class AddRecipeDto
    {
        public string ParentAncestryPath { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
