using Cookbook.API.Models;
using Cookbook.API.Utils;
using Cookbook.Repository;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Cookbook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase, IDisposable
    {
        public RecipesController(IRecipesRepository repo = null) => 
            _repo = repo ?? new RecipesRepository();

        /// <summary>Returns recipes for given parent ID</summary>
        /// <param name="parentId">Id of the parent recipe we want to fetch children for</param>
        /// <returns>List of child recipes for given parent Id. Empty list, if recipe doesnt have any children</returns>
        [HttpGet("{parentId?}")]
        public IActionResult GetRecipes(int parentId = 0)
        {
            if (parentId < 0)
                return new BadRequestResult();

            var result = _repo.GetRecipes(parentId)?.ToRecipeDto();

            if (result is null)
                return new NotFoundResult();
            else
                return new JsonResult(result);
        }

        /// <summary>Returns recipe history for given recipe ID</summary>
        /// <param name="recipeId">ID of the recipe we want to fetch log entries for</param>
        /// <returns>List of log entries for given ID</returns>
        [HttpGet("{recipeId}/history")]
        public IActionResult GetRecipeHistory(int recipeId)
        {
            if (recipeId < 0)
                return new BadRequestResult();
           
            var result = _repo.GetLogEntries(recipeId)?.ToLogEntryDto();
            return new JsonResult(result);        
        }

        /// <summary>Updates exisiting recipe</summary>
        /// <returns>Ok if successful, Unprocessable Entity otherwise</returns>
        [HttpPut]
        public IActionResult UpdateRecipe(UpdateRecipeDto recipe)
        {
            if (_repo.UpdateRecipe(recipe.Title, recipe.Description, recipe.RecipeID))
                return new OkResult();
            else
                return new UnprocessableEntityResult();
        }

        /// <summary>Adds a new recipe if its parent path is valid</summary>
        /// <returns>Ok if successful, Unprocessable Entity otherwise</returns>
        [HttpPost]
        public IActionResult AddRecipe(NewRecipeDto recipe)
        {
            if (_repo.AddRecipe(recipe.Title, recipe.Description, recipe.ParentAncestryPath) > 0)
                return new OkResult();
            else 
                return new UnprocessableEntityResult();
        }

        public void Dispose() => _repo.Dispose();

        private IRecipesRepository _repo;
    }
}
