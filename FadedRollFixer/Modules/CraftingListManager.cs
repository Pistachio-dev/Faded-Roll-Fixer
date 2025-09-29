using FadedRollFixer.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            // Order by item id
            List<KeyValuePair<uint, int>> orderedKeys = craftingList.OrderBy(p => p.Key).ToList();

            foreach (var keyValuePair in orderedKeys)
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
