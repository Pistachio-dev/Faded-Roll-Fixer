using Dalamud.Game.Network.Structures;
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
        
        public static (Dictionary<uint, int> FinalItems, Dictionary<uint, int> IntermediateItems)
            GetCraftingList(Dictionary<uint, int> fadedScrolls)
        {
            GetRecipes(fadedScrolls, out List<Recipe> unregisteredRecipes, out List<Recipe> complexRecipes, out List<Recipe> otherRecipes);
            return GetCraftingListInternal(ref fadedScrolls, unregisteredRecipes, complexRecipes, otherRecipes);
        }



        private unsafe static void GetRecipes(Dictionary<uint, int> fadedScrolls,
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

        private static (Dictionary<uint, int> FinalItems, Dictionary<uint, int> IntermediateItems)
            GetCraftingListInternal(ref Dictionary<uint, int> fadedScrolls, 
            List<Recipe> unregisteredRecipes, List<Recipe> complexRecipes, List<Recipe> otherRecipes)
        {
            var finalItemList = new Dictionary<uint, int>();
            var intermediateItemList = new Dictionary<uint, int>();
            ApplyRecipeList(ref finalItemList, ref intermediateItemList, ref fadedScrolls, unregisteredRecipes);
            int sanityCounter = 0; // Just to avoid accidental infinite loops
            while (fadedScrolls.Any() && sanityCounter < 100)
            {
                ApplyRecipeList(ref finalItemList, ref intermediateItemList, ref fadedScrolls, complexRecipes);
                ApplyRecipeList(ref finalItemList, ref intermediateItemList, ref fadedScrolls, otherRecipes);
                sanityCounter++;
            }

            return (finalItemList, intermediateItemList);
        }

        private static void ApplyRecipeList(ref Dictionary<uint, int> finalItemList, ref Dictionary<uint, int> intermediateItemList,
            ref Dictionary<uint, int> fadedScrolls, List<Recipe> recipes)
        {
            foreach(var recipe in recipes)
            {
                if (!fadedScrolls.Any())
                {
                    return;
                }

                if (ContainsIngredients(recipe, fadedScrolls))
                {
                    Item result = ApplyRecipe(recipe, ref fadedScrolls);
                    finalItemList.AddToCount(result.RowId, 1);
                    for (int i = 0; i < recipe.Ingredient.Count; i++)
                    {
                        var item = recipe.Ingredient[i].Value;
                        if (!ItemHelpers.IsFadedOrchestrionRoll(item) 
                            && (item.Name.ToString().Contains("Ink", StringComparison.OrdinalIgnoreCase)
                                || item.Name.ToString().Contains("Blank Grade ", StringComparison.OrdinalIgnoreCase)))
                        {
                            intermediateItemList.AddToCount(item.RowId, recipe.AmountIngredient[i]);
                        }
                    }
                }
            }
        }

        private static bool ContainsIngredients(Recipe recipe, Dictionary<uint, int> fadedScrolls)
        {
            foreach (var ingredient in recipe.Ingredient)
            {
                if (ItemHelpers.IsFadedOrchestrionRoll(ingredient.Value) 
                    && (!fadedScrolls.TryGetValue(ingredient.Value.RowId, out int amount) || amount == 0))
                {
                    return false;
                }
            }

            return true;
        }

        private static Item ApplyRecipe(Recipe recipe, ref Dictionary<uint, int> fadedScrolls)
        {
            foreach (var ingredient in recipe.Ingredient)
            {
                if (ItemHelpers.IsFadedOrchestrionRoll(ingredient.Value))
                {
                    fadedScrolls[ingredient.Value.RowId] = fadedScrolls[ingredient.Value.RowId] - 1;
                    if (fadedScrolls[ingredient.Value.RowId] == 0)
                    {
                        fadedScrolls.Remove(ingredient.Value.RowId);
                    }
                }
            }

            return recipe.ItemResult.Value;
        }

        private static void DumpRecipeList(string listIdentifier, List<Recipe> recipes)
        {
            Plugin.Log.Info($"{listIdentifier}-------------------------");
            foreach (var recipe in recipes)
            {
                Plugin.Log.Info(recipe.ItemResult.Value.Name.ToString());
            }
        }
    }
}
