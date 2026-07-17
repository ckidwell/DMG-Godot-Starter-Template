using Godot;
using System;

namespace MyGame;

public partial class QuitMenu : CanvasLayer
{
    [Export] private TextureButton _confirmButton;
    [Export] private TextureButton _cancelButton;

    private GameEvents _gameEvents;
    private MenuSystemManager _menuSystemManager;
    
    public override void _Ready()
    {
        _gameEvents = GetNode<GameEvents>("/root/GameEvents");
        _menuSystemManager = GetNode<MenuSystemManager>("/root/MenuSystemManager");
        
        _confirmButton.Pressed += OnConfirmButtonPressed;
        _cancelButton.Pressed += OnCancelButtonPressed;
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
