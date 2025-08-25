using Dalamud.Utility;
using Lumina.Excel.Sheets;
using System.Collections.Generic;
using System.Linq;
using Action = Lumina.Excel.Sheets.Action;
using Status = Lumina.Excel.Sheets.Status;

// Taken from Artisan, most of it at least
namespace FadedRollFixer.Data
{
    public class LuminaSheets
    {

        public static Dictionary<uint, Recipe>? RecipeSheet;

        public static ILookup<string, Recipe>? recipeLookup;


        public static Dictionary<uint, GatheringItem>? GatheringItemSheet;

        public static Dictionary<uint, SpearfishingItem>? SpearfishingItemSheet;

        public static Dictionary<uint, GatheringPointBase>? GatheringPointBaseSheet;

        public static Dictionary<uint, FishParameter>? FishParameterSheet;

        public static Dictionary<uint, ClassJob>? ClassJobSheet;

        public static Dictionary<uint, Item>? ItemSheet;

        public static Dictionary<uint, Action>? ActionSheet;

        public static Dictionary<uint, Status>? StatusSheet;

        public static Dictionary<uint, CraftAction>? CraftActions;

        public static Dictionary<uint, RecipeLevelTable>? RecipeLevelTableSheet;

        public static Dictionary<uint, Addon>? AddonSheet;

        public static Dictionary<uint, SpecialShop>? SpecialShopSheet;

        public static Dictionary<uint, LogMessage>? LogMessageSheet;

        public static Dictionary<uint, ItemFood>? ItemFoodSheet;

        public static Dictionary<uint, ENpcResident>? ENPCResidentSheet;

        public static Dictionary<uint, Quest>? QuestSheet;

        public static Dictionary<uint, CompanyCraftPart>? WorkshopPartSheet;

        public static Dictionary<uint, CompanyCraftProcess>? WorkshopProcessSheet;

        public static Dictionary<uint, CompanyCraftSequence>? WorkshopSequenceSheet;

        public static Dictionary<uint, CompanyCraftSupplyItem>? WorkshopSupplyItemSheet;

        public static void Init()
        {
            RecipeSheet = Plugin.DataManager?.GetExcelSheet<Recipe>()?
           .Where(x => x.ItemResult.RowId > 0)
                .DistinctBy(x => x.RowId)
                .OrderBy(x => x.RecipeLevelTable.Value.ClassJobLevel)
                .ThenBy(x => x.ItemResult.Value.Name.ToDalamudString().ToString())
                .ToDictionary(x => x.RowId, x => x);

            // Preprocess the recipe data into a lookup table (ILookup) for faster access.
            recipeLookup = RecipeSheet.Values
                .ToLookup(x => x.ItemResult.Value.Name.ToDalamudString().ToString());

            GatheringItemSheet = Plugin.DataManager?.GetExcelSheet<GatheringItem>()?
                .Where(x => x.GatheringItemLevel.Value.GatheringItemLevel > 0)
                .ToDictionary(i => i.RowId, i => i);

            SpearfishingItemSheet = Plugin.DataManager?.GetExcelSheet<SpearfishingItem>()?
                .Where(x => x.GatheringItemLevel.Value.GatheringItemLevel > 0)
                .ToDictionary(i => i.RowId, i => i);

            GatheringPointBaseSheet = Plugin.DataManager?.GetExcelSheet<GatheringPointBase>()?
               .Where(x => x.GatheringLevel > 0)
               .ToDictionary(i => i.RowId, i => i);

            FishParameterSheet = Plugin.DataManager?.GetExcelSheet<FishParameter>()?
                 .Where(x => x.GatheringItemLevel.Value.GatheringItemLevel > 0)
                 .ToDictionary(i => i.RowId, i => i);

            ClassJobSheet = Plugin.DataManager?.GetExcelSheet<ClassJob>()?
                       .ToDictionary(i => i.RowId, i => i);

            ItemSheet = Plugin.DataManager?.GetExcelSheet<Item>()?
                       .ToDictionary(i => i.RowId, i => i);

            ActionSheet = Plugin.DataManager?.GetExcelSheet<Action>()?
                        .ToDictionary(i => i.RowId, i => i);

            StatusSheet = Plugin.DataManager?.GetExcelSheet<Status>()?
                       .ToDictionary(i => i.RowId, i => i);

            CraftActions = Plugin.DataManager?.GetExcelSheet<CraftAction>()?
                       .ToDictionary(i => i.RowId, i => i);

            RecipeLevelTableSheet = Plugin.DataManager?.GetExcelSheet<RecipeLevelTable>()?
                       .ToDictionary(i => i.RowId, i => i);

            AddonSheet = Plugin.DataManager?.GetExcelSheet<Addon>()?
                       .ToDictionary(i => i.RowId, i => i);

            SpecialShopSheet = Plugin.DataManager?.GetExcelSheet<SpecialShop>()?
                       .ToDictionary(i => i.RowId, i => i);

            LogMessageSheet = Plugin.DataManager?.GetExcelSheet<LogMessage>()?
                       .ToDictionary(i => i.RowId, i => i);

            ItemFoodSheet = Plugin.DataManager?.GetExcelSheet<ItemFood>()?
                       .ToDictionary(i => i.RowId, i => i);

            ENPCResidentSheet = Plugin.DataManager?.GetExcelSheet<ENpcResident>()?
                       .Where(x => x.Singular.ExtractText().Length > 0)
                       .ToDictionary(i => i.RowId, i => i);

            QuestSheet = Plugin.DataManager?.GetExcelSheet<Quest>()?
                        .Where(x => x.Id.ExtractText().Length > 0)
                        .ToDictionary(i => i.RowId, i => i);

            WorkshopPartSheet = Plugin.DataManager?.GetExcelSheet<CompanyCraftPart>()?
                       .ToDictionary(i => i.RowId, i => i);

            WorkshopProcessSheet = Plugin.DataManager?.GetExcelSheet<CompanyCraftProcess>()?
                       .ToDictionary(i => i.RowId, i => i);

            WorkshopSequenceSheet = Plugin.DataManager?.GetExcelSheet<CompanyCraftSequence>()?
                       .ToDictionary(i => i.RowId, i => i);

            WorkshopSupplyItemSheet = Plugin.DataManager?.GetExcelSheet<CompanyCraftSupplyItem>()?
                       .ToDictionary(i => i.RowId, i => i);

            Plugin.Log.Debug("Lumina sheets initialized");
        }

        public static void Dispose()
        {
            var type = typeof(LuminaSheets);
            foreach (var prop in type.GetFields(System.Reflection.BindingFlags.Static))
            {
                prop.SetValue(null, null);
            }
        }
    }

    public static class SheetExtensions
    {

        public static string NameOfBuff(this ushort id)
        {
            if (id == 0) return "";

            return LuminaSheets.StatusSheet[id].Name.ToString();
        }

        public static string NameOfItem(this uint id)
        {
            if (id == 0) return "";

            return LuminaSheets.ItemSheet[id].Name.ExtractText();
        }

        public static string NameOfRecipe(this uint id)
        {
            if (id == 0) return "";
            if (!LuminaSheets.RecipeSheet.ContainsKey(id))
                return "";

            return LuminaSheets.RecipeSheet[id].ItemResult.Value.Name.ToDalamudString().ToString();
        }
    }
}
