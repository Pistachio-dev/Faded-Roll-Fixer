using FadedRollFixer.Data;
using FFXIVClientStructs.FFXIV.Client.Game;
using Lumina.Excel.Sheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FadedRollFixer.Modules
{
    public static class InventoryReader
    {
        public static unsafe Dictionary<uint, int> GetAvailableFadedOrchestrions()
        {
            Dictionary<uint, int> availableOrchestrions = new();
            foreach (var inventory in GetCombinedInventories())
            {
                for (var i = 0; i < inventory->Size; i++)
                {
                    var item = inventory->Items[i];
                    uint itemId = item.ItemId;
                    if (LuminaSheets.ItemSheet == null)
                    {
                        Plugin.Log.Warning("Item sheet is null");
                        return availableOrchestrions;
                    }
                    var itemInfo = LuminaSheets.ItemSheet[itemId];
                    if (itemInfo.ItemUICategory.RowId == 94 && itemInfo.Name.ToString().Contains("faded", StringComparison.OrdinalIgnoreCase))
                    {
                        availableOrchestrions.AddToCount(itemInfo.RowId, item.Quantity);
                    }
                }
            }

            return availableOrchestrions;
        }

        internal static unsafe InventoryContainer*[] GetCombinedInventories()
        {
            var im = InventoryManager.Instance();
            InventoryContainer*[] container = {
                im->GetInventoryContainer(InventoryType.Inventory1),
                im->GetInventoryContainer(InventoryType.Inventory2),
                im->GetInventoryContainer(InventoryType.Inventory3),
                im->GetInventoryContainer(InventoryType.Inventory4),
            };

            return container;
        }
    }
}
