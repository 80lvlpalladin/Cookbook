namespace Cookbook.API.Models
{
    public class NewRecipeDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ParentAncestryPath { get; set; }
    }
}
