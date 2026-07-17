using Godot;
using System;

namespace DMGStarterTemplate;

public partial class QuitMenu : CanvasLayer
{
    [Export] private TextureButton _confirmButton;
    [Export] private TextureButton _cancelButton;

    private GameEvents _gameEvents;
    private MenuSystemManager _menuSystemManager;
    
    public override void _Ready()
    {
        // Stay interactive while the game tree is paused so the buttons still respond.
        ProcessMode = ProcessModeEnum.Always;

        _gameEvents = GetNode<GameEvents>("/root/GameEvents");
        _menuSystemManager = GetNode<MenuSystemManager>("/root/MenuSystemManager");

        _confirmButton.Pressed += OnConfirmButtonPressed;
        _cancelButton.Pressed += OnCancelButtonPressed;
    }

    // Pausing/unpausing is driven by this menu entering/leaving the tree, so every open and
    // close path (pause toggle, Cancel, Confirm) is covered by a single pair of hooks.
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
        _gameEvents.EmitPlayAudioStream(GameConstants.UI_CLICK_BUTTON);
        _menuSystemManager.PopMenu(MenuType.PAUSE_QUIT);
        _menuSystemManager.SetCurrentMenu(MenuType.MAIN);
    }

    private void OnCancelButtonPressed()
    {
        _gameEvents.EmitPlayAudioStream(GameConstants.UI_CLICK_BUTTON);
        _menuSystemManager.PopMenu(MenuType.PAUSE_QUIT);
    }
    
}
