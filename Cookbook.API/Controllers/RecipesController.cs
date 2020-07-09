using Cookbook.API.Models;
using Cookbook.API.Utils;
using Cookbook.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Cookbook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        /// <summary>Returns recipes for given parent ID</summary>
        /// <param name="parentId">Id of the parent recipe we want to fetch children for</param>
        /// <returns>List of child recipes for given parent Id. Empty list, if recipe doesnt have any children</returns>
        [HttpGet("{parentId?}")]
        public ActionResult<IEnumerable<RecipeDto>> GetRecipes(int parentId = 0)
        {
            if (parentId < 0)
                return new BadRequestResult();

            using var reader = new CookbookReader();
            var result = reader.GetRecipes(parentId)?.ToRecipeDto();

            if (result is null)
                return new NotFoundResult();
            else
                return new JsonResult(result);
        }

        [HttpGet("{recipeId}/history")]
        public ActionResult<IEnumerable<RecipeLogEntryDto>> GetRecipeHistory(int recipeId)
        {
            if (recipeId < 0)
                return new BadRequestResult();
            using var reader = new CookbookReader();
            var result = reader.GetLogEntries(recipeId)?.ToLogEntryDto();

            if (result is null)
                return new NotFoundResult();
            else
                return new JsonResult(result);
        }

        [HttpPut]
        public IActionResult UpdateRecipe(UpdateRecipeDto recipe)
        {
            using var writer = new CookbookWriter();

            if (writer.UpdateRecipe(recipe.Title, recipe.Description, recipe.RecipeID))
                return new OkResult();
            else
                return new UnprocessableEntityResult();
        }

        [HttpPost]
        public IActionResult AddRecipe(NewRecipeDto recipe)
        {
            using var writer = new CookbookWriter();

            if (writer.AddRecipe(recipe.Title, recipe.Description, recipe.ParentAncestryPath))
                return new OkResult();
            else 
                return new UnprocessableEntityResult();
        }
    }
}
