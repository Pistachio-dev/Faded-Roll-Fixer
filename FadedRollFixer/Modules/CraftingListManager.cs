using FadedRollFixer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FadedRollFixer.Modules
{
    public static class CraftingListManager
    {
        public static string GetCraftingListForFadedRollsInInventory()
        {
            var fadedOrchestrions = InventoryReader.GetAvailableFadedOrchestrions();
            var craftingList = RecipeManager.GetCraftingList(fadedOrchestrions);
            StringBuilder s = new StringBuilder("Items :\n");
            foreach (var keyValuePair in craftingList)
            {
                s.Append(keyValuePair.Value);
                s.Append("x ");
                var name = LuminaSheets.ItemSheet[keyValuePair.Key].Name.ToString();
                s.Append(name);
                s.Append("\n");
            }

            return s.ToString();
        }
    }
}
