using System;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using FadedRollFixer.Modules;
using Humanizer;
using Lumina.Excel.Sheets;

namespace FadedRollFixer.Windows;

public class MainWindow : Window, IDisposable
{
    private Plugin Plugin;
    private string craftingList = string.Empty;

    // We give this window a hidden ID using ##.
    // The user will see "My Amazing Window" as window title,
    // but for ImGui the ID is "My Amazing Window##With a hidden ID"
    public MainWindow(Plugin plugin)
        : base("Faded Orchestrion Repair Shop##With a hidden ID", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        Plugin = plugin;
    }

    public void Dispose() { }

    public override void Draw()
    {
        if (ImGui.Button("Get crafting list"))
        {
            craftingList = CraftingListManager.GetCraftingListForFadedRollsInInventory();
        }

        ImGui.Spacing();

        // Normally a BeginChild() would have to be followed by an unconditional EndChild(),
        // ImRaii takes care of this after the scope ends.
        // This works for all ImGui functions that require specific handling, examples are BeginTable() or Indent().
        using (var child = ImRaii.Child("SomeChildWithAScrollbar", Vector2.Zero, true))
        {
            // Check if this child is drawing
            if (child.Success)
            {
                ImGui.TextUnformatted(craftingList);
                if (ImGui.IsItemClicked())
                {
                    ImGui.SetClipboardText(craftingList);
                    Plugin.ChatGui.Print("List copied to clipboard");
                }                
            }
        }
    }
}
