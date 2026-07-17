using Godot;
using System;

namespace DMGStarterTemplate;

public partial class MainMenu : CanvasLayer
{
    [Export] private Control _visualControlParent;
    [Export] private TextureButton _playButton;
    [Export] private TextureButton _settingsButton;
    [Export] private TextureButton _quitButton;
    [Export] private TextureButton _achievementsButton;

    private GameEvents _gameEvents;
    private MenuSystemManager _menuSystemManager;
    
    public override void _Ready()
    {
        _gameEvents = GetNode<GameEvents>("/root/GameEvents");
        _menuSystemManager = GetNode<MenuSystemManager>("/root/MenuSystemManager");
        
        _playButton.Pressed += OnPlayButtonPressed;
        _settingsButton.Pressed += OnSettingsButtonPressed;
        _quitButton.Pressed += OnQuitButtonPressed;
        _achievementsButton.Pressed += OnAchievementsButtonPressed;
    }

    private void OnAchievementsButtonPressed()
    {
        _gameEvents.EmitPlayAudioStream(GameConstants.UI_CLICK_BUTTON);
        _menuSystemManager.SetCurrentMenu(MenuType.ACHIEVEMENTS);
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
        // DEMO: fire the achievement toast on every Play press so template users can see what an
        // earned achievement and its toast notification look like. This deliberately bypasses
        // ProgressionManager.AchievementUnlocked() — that path persists the unlock and would only
        // ever show the toast once. Replace this with real unlock logic in your own game.
        _gameEvents.EmitAchievementEarned(new AchievementDescriptionVariant( AchievementDescription.GetDescriptionForAchievement(Achievements.WELCOME_FIRST_TIME)));
        _gameEvents.EmitPlayAudioStream(GameConstants.UI_CLICK_BUTTON);
        _menuSystemManager.SetCurrentMenu(MenuType.PLAY);
    }

}
