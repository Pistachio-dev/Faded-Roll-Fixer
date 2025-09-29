using Lumina.Excel.Sheets;
using System.Collections.Generic;

namespace FadedRollFixer.Data
{
    public class AvailableRecipes
    {
        public bool HasMultiScrollRecipe()
        {
            foreach (var recipe in Recipes)
            {
                if (recipe.Ingredient.Count > 3)
                {
                    return true;
                }
            }

            return false;
        }

        public List<Recipe> Recipes { get; set; } = new();
    }
}
