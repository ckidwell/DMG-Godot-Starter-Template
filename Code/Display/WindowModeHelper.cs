using Godot;

namespace DMGStarterTemplate;

public static class WindowModeHelper
{
    public static bool IsWindowed =>
        DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Windowed;

    public static void SetWindowed(bool windowed)
    {
        if (windowed)
        {
            DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
        }
        else
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
        }
    }
}
