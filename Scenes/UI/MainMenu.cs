using Godot;
using System;

public partial class MainMenu : CanvasLayer
{
    [Export] private Control _visualControlParent;
    [Export] private TextureButton _playButton;
    [Export] private TextureButton _settingsButton;
    [Export] private TextureButton _quitButton;

    private GameEvents _gameEvents;
    private MenuSystemManager _menuSystemManager;
    
    public override void _Ready()
    {
        _gameEvents = GetNode<GameEvents>("/root/GameEvents");
        _menuSystemManager = GetNode<MenuSystemManager>("/root/MenuSystemManager");
        
        _playButton.Pressed += OnPlayButtonPressed;
        _settingsButton.Pressed += OnSettingsButtonPressed;
        _quitButton.Pressed += OnQuitButtonPressed;
    }

    

    private void OnQuitButtonPressed()
    {
        _gameEvents.EmitPlayAudioStream(GameConstants.UI_CLICK_BUTTON);
        GetTree().Quit();
    }

    private void OnSettingsButtonPressed()
    {
        _gameEvents.EmitPlayAudioStream(GameConstants.UI_CLICK_BUTTON);
        _menuSystemManager.SetCurrentMenu(MenuType.SETTINGS);
    }

    private void OnPlayButtonPressed()
    {
        _gameEvents.EmitAchievementEarned(new AchievementDescriptionVariant( AchievementDescription.GetDescriptionForAchievement(Achievements.WELCOME_FIRST_TIME)));
        _gameEvents.EmitPlayAudioStream(GameConstants.UI_CLICK_BUTTON);
        _menuSystemManager.SetCurrentMenu(MenuType.PLAY);
    }

}
