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
        public static (string final, string intermediate) GetCraftingListForFadedRollsInInventory()
        {
            var fadedOrchestrions = InventoryReader.GetAvailableFadedOrchestrions();
            (var finalItemList, var intermediateList) = RecipeManager.GetCraftingList(fadedOrchestrions);
            string finalItemsTextList = GetList("Items", finalItemList);
            string intermediateTextList = GetList("Pre crafts", intermediateList);

            return (finalItemsTextList, intermediateTextList);
        }

        private static string GetList(string header, Dictionary<uint, int> craftingList)
        {
            StringBuilder s = new StringBuilder($"{header} :\n");

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
