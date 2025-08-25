using FadedRollFixer.Data;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Lumina.Excel.Sheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FadedRollFixer.Modules
{
    public static class RecipeManager
    {
        
        public unsafe static void GetRecipes(Dictionary<uint, int> fadedScrolls,
            out List<Recipe> unregisteredRecipes, out List<Recipe> complexRecipes, out List<Recipe> otherRecipes)
        {
            unregisteredRecipes = new();
            complexRecipes = new();
            otherRecipes = new();
            foreach (var keyValuePair in fadedScrolls)
            {
                var recipes = LuminaSheets.IngredientIdToRecipeLookup[keyValuePair.Key];
                foreach (var recipe in recipes)
                {
                    if (!UIState.Instance()->PlayerState.IsOrchestrionRollUnlocked(recipe.ItemResult.Value.AdditionalData.RowId))
                    {
                        unregisteredRecipes.Add(recipe);
                    }
                    if (recipe.Ingredient.Count > 3)
                    {
                        complexRecipes.Add(recipe);                        
                    }
                    else
                    {
                        otherRecipes.Add(recipe);
                    }
                }
            }
        }
    }
}
