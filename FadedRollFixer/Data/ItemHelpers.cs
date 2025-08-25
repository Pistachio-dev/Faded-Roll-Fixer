using Lumina.Excel.Sheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FadedRollFixer.Data
{
    public static class ItemHelpers
    {
        public static bool IsOrchestrionRoll(Item item)
        {
            return item.ItemUICategory.RowId == 94 && !item.Name.ToString().Contains("Blank", System.StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsFadedOrchestrionRoll(Item item)
        {
            return item.ItemUICategory.RowId == 94 && item.Name.ToString().Contains("Faded", System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
