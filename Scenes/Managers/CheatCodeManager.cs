using Godot;
using System;

public partial class CheatCodeManager : Node
{
    [Export] private bool _cheatEnabled = false;

    private GameEvents _gameEvents;

    private bool _screenShakeComboHeld;


    public override void _Ready()
    {
        _gameEvents = GetNode<GameEvents>("/root/GameEvents");
      
    }

    public override void _Process(double delta)
    {

        if (!_cheatEnabled) return;

        CheckCheatCodes();
    }

    private void CheckCheatCodes()
    {
        // CTRL+S: trigger a strong screen shake.
        var screenShakeComboHeld = Input.IsKeyPressed(Key.Ctrl) && Input.IsKeyPressed(Key.S);

        // fire once on the rising edge so it doesn't repeat every frame while held
        if (screenShakeComboHeld && !_screenShakeComboHeld)
        {
            // duration, strength, strengthDecayRate, rampTime, rampStrength
            _gameEvents.EmitScreenShake(0.8f, 150f, 3f, 0f, 0f);
        }

        _screenShakeComboHeld = screenShakeComboHeld;
    }
}
