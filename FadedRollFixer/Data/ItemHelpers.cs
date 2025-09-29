using Lumina.Excel.Sheets;

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
