using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cookbook.Client.Models
{
    /// <summary>Class responsible for consuming Cookbook.API</summary>
    public class APIConsumer : IDisposable
    {
        private readonly HttpClient _client;
        private readonly string _baseAddress;

        public async Task<IEnumerable<Recipe>> GetRecipesAsync(int parentId = 0)
        {
            if (parentId < 0)
                throw new ArgumentOutOfRangeException("Recipe ID cannot be less than 0");

            using var response = 
                await _client.GetAsync(_baseAddress + "/api/recipes" + (parentId == 0 ? "" : $"/{parentId}"));

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<IEnumerable<Recipe>>();
                return result;
            }

            else throw new Exception(response.ReasonPhrase);
        }

        public APIConsumer()
        {
            _client = new HttpClient();
            _baseAddress = "https://localhost:5001";
        }

        ///<inheritdoc/>
        public void Dispose() => _client.Dispose();
    }
}
