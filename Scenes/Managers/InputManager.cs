using Godot;

namespace DMGStarterTemplate;

public partial class InputManager : Node
{
    private MenuSystemManager _menuSystemManager;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;

        _menuSystemManager = GetNode<MenuSystemManager>("/root/MenuSystemManager");
    }
    
    public override void _UnhandledInput(InputEvent @event)
    {
        if (!@event.IsActionPressed("pause")) return;
        
        if (_menuSystemManager.CurrentMenu != MenuType.PLAY) return;

        _menuSystemManager.ToggleMenu(MenuType.PAUSE_QUIT);
        GetViewport().SetInputAsHandled();
    }
}
