using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;
using System;
using System.Numerics;

namespace FadedRollFixer.Windows;

public class ConfigWindow : Window, IDisposable
{
    public ConfigWindow(Plugin plugin) : base($"Configuration###{Guid.NewGuid()}")
    {
        Size = new Vector2(250, 60);
        SizeCondition = ImGuiCond.Always;
    }

    public void Dispose()
    { }

    public override void Draw()
    {
        ImGui.TextUnformatted("There's nothing to configure.");
    }
}
