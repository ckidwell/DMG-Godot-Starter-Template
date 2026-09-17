using Godot;

namespace DMGStarterTemplate;

// Debug cheats. Each cheat is an InputMap action (see project.godot [input]) handled in _UnhandledInput,
// Cheats only work in debug builds 
public partial class CheatCodeManager : Node
{
    private GameEvents _gameEvents;

    public override void _Ready()
    {
        _gameEvents = GetNode<GameEvents>("/root/GameEvents");
        
        SetProcessUnhandledInput( OS.IsDebugBuild());
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        // CTRL+S: trigger a strong screen shake.
        if (@event.IsActionPressed("cheat_screen_shake"))
        {
            // duration, strength, strengthDecayRate
            _gameEvents.EmitScreenShake(0.8f, 150f, 3f);
            GetViewport().SetInputAsHandled();
        }
    }
}
