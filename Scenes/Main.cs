using Godot;
using System;

public partial class Main : Control
{
    [Export] public PackedScene mainMenu;
    [Export] public PackedScene settingsMenu;
    [Export] public PackedScene quitGameMenu;
    [Export] public PackedScene achievementsMenu;
    [Export] public PackedScene gamePlayScene;
    

    private MenuSystemManager _menuSystemManager;
    private GameEvents _gameEvents;
    
    
    public override void _Ready()
    {
        base._Ready();
        _menuSystemManager = GetNode<MenuSystemManager>("/root/MenuSystemManager");

        InitalizeMenus();
        _menuSystemManager.SetCurrentMenu(MenuType.MAIN);
    }

    private void InitalizeMenus()
    {
        _menuSystemManager.InitializeMenu(MenuType.MAIN,  mainMenu.Instantiate());
        _menuSystemManager.InitializeMenu(MenuType.SETTINGS, settingsMenu.Instantiate());
        _menuSystemManager.InitializeMenu(MenuType.PAUSE_QUIT, quitGameMenu.Instantiate());
        _menuSystemManager.InitializeMenu(MenuType.PLAY, gamePlayScene.Instantiate());
        _menuSystemManager.InitializeMenu(MenuType.ACHIEVEMENTS, achievementsMenu.Instantiate());
        
    }
}
