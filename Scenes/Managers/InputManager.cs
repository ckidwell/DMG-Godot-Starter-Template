using Godot;
using System;

namespace DMGStarterTemplate;

public partial class InputManager : Node
{
    private GameEvents _gameEvents;
    private MenuSystemManager _menuSystemManager;
    
    public override void _Ready()
    {
        // Keep handling input while the tree is paused, so pause can also un-pause.
        ProcessMode = ProcessModeEnum.Always;

        _gameEvents = GetNode<GameEvents>("/root/GameEvents");
        _menuSystemManager = GetNode<MenuSystemManager>("/root/MenuSystemManager");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("pause"))
        {
            _menuSystemManager.ToggleMenu(MenuType.PAUSE_QUIT);
        }
    }
}
