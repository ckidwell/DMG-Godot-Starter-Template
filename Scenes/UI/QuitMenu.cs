using Godot;
using System;

namespace DMGStarterTemplate;

public partial class QuitMenu : CanvasLayer
{
    [Export] private TextureButton _confirmButton;
    [Export] private TextureButton _cancelButton;

    private MenuSystemManager _menuSystemManager;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;

        _menuSystemManager = GetNode<MenuSystemManager>("/root/MenuSystemManager");

        _confirmButton.Pressed += OnConfirmButtonPressed;
        _cancelButton.Pressed += OnCancelButtonPressed;
    }

    public override void _EnterTree()
    {
        GetTree().Paused = true;
    }

    public override void _ExitTree()
    {
        GetTree().Paused = false;
    }

    private void OnConfirmButtonPressed()
    {
        _menuSystemManager.PopMenu(MenuType.PAUSE_QUIT);
        _menuSystemManager.SetCurrentMenu(MenuType.MAIN);
    }

    private void OnCancelButtonPressed()
    {
        _menuSystemManager.PopMenu(MenuType.PAUSE_QUIT);
    }
    
}
