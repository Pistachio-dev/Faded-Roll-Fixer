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
    private string finalCraftingList = string.Empty;
    private string intermediateComponentsList = string.Empty;

    // We give this window a hidden ID using ##.
    // The user will see "My Amazing Window" as window title,
    // but for ImGui the ID is "My Amazing Window##With a hidden ID"
    public MainWindow(Plugin plugin)
        : base("Faded Orchestrion Repair Shop##With a hidden ID", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(400, 630),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        Plugin = plugin;
    }

    public void Dispose() { }

    public override void Draw()
    {
        ImGui.TextUnformatted("This will work based on the faded orchestrion rolls in your inventory");
        if (ImGui.Button("Get crafting lists"))
        {
            (finalCraftingList, intermediateComponentsList) = CraftingListManager.GetCraftingListForFadedRollsInInventory();
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
                ImGui.TextUnformatted(intermediateComponentsList);
                if (ImGui.IsItemClicked())
                {
                    ImGui.SetClipboardText(intermediateComponentsList);
                    Plugin.ChatGui.Print("List copied to clipboard");
                }
                ImGui.Separator();
                ImGui.TextUnformatted(finalCraftingList);
                if (ImGui.IsItemClicked())
                {
                    ImGui.SetClipboardText(finalCraftingList);
                    Plugin.ChatGui.Print("List copied to clipboard");
                }                
            }
        }
    }
}
