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
        [HttpGet]
        public ActionResult<IEnumerable<RecipeDto>> GetRecipes()
        {
            using var reader = new CookbookReader();
            return new JsonResult(reader.GetRecipes().ToRecipeDto());
        }
    }
}
