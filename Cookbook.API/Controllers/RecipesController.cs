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
            using var reader = new CookbookReader();
            var result = reader.GetRecipes(parentId)?.ToRecipeDto();

            if (result is null)
                return new NotFoundResult();
            else
                return new JsonResult(result);
        }
    }
}
