using Godot;
using System;

namespace MyGame;

public partial class InputManager : Node
{
    private GameEvents _gameEvents;
    private MenuSystemManager _menuSystemManager;
    
    public override void _Ready()
    {
        _gameEvents = GetNode<GameEvents>("/root/GameEvents");
        _menuSystemManager = GetNode<MenuSystemManager>("/root/MenuSystemManager");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("pause"))
        {
            _menuSystemManager.PushMenu(MenuType.PAUSE_QUIT);
        }
    }
}
