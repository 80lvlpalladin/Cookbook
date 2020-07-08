using Cookbook.Client.Models.DTOs;
using Cookbook.Client.Utils;
using Cookbook.Client.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Client.Models
{
    /// <summary>Class responsible for consuming Cookbook.API</summary>
    public class APIConsumer
    {
        private static readonly HttpClient _client = new HttpClient()
        {
            BaseAddress = GlobalStrings.APIHostAddress
        };

        public static async Task SendNewRecipeAsync(NewRecipeDto recipe)
        {           
            var result = await _client.PostAsJsonAsync("api/recipes", recipe);

            if (!result.IsSuccessStatusCode)
                throw new Exception(result.ReasonPhrase);
        }

        public static async Task UpdateRecipeAsync(UpdateRecipeDto recipe)
        {
            var result = await _client.PutAsJsonAsync("api/recipes", recipe);

            if (!result.IsSuccessStatusCode)
                throw new Exception(result.ReasonPhrase);
        }

        public static async Task<IEnumerable<RecipeViewModel>> GetRecipesAsync(int parentId = 0)
        {
            if (parentId < 0)
                throw new ArgumentOutOfRangeException("Recipe ID cannot be less than 0");

            using var response = await
                 _client.GetAsync("/api/recipes" + (parentId == 0 ? "" : $"/{parentId}/"));

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<IEnumerable<RecipeViewModel>>();
                return result;
            }

            else throw new Exception(response.ReasonPhrase);
        }
    }
}
