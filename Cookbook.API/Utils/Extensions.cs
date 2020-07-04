using Cookbook.API.Models;
using Cookbook.Repository.Entities;
using System;
using System.Collections.Generic;

namespace Cookbook.API.Utils
{
    public static class Extensions
    {
        public static IEnumerable<RecipeDto> ToRecipeDto(this Dictionary<RecipeNode, RecipeLogEntry> dict)
        {
            foreach (var keyvalue in dict)
            {
                if (keyvalue.Key.RecipeID != keyvalue.Value.RecipeID)
                    throw new ArgumentException("RecipeID in RecipeNode and in RecipeLogEntry MUST be equal");

                yield return new RecipeDto()
                {
                    RecipeID = keyvalue.Key.RecipeID,
                    AncestryPath = keyvalue.Key.AncestryPath,
                    LastUpdated = keyvalue.Value.LastUpdated,
                    Title = keyvalue.Value.Title,
                    Description = keyvalue.Value.Description
                };
            }
        }
    }
}
