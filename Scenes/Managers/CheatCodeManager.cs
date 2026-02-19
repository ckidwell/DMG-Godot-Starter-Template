using Godot;
using System;

namespace MyGame;

public partial class CheatCodeManager : Node
{
    [Export] private bool _cheatEnabled = false;
    
    private GameEvents _gameEvents;
    

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
        // if (Input.IsKeyPressed(Key.X))
        // {
        //     
        // }
        // if (Input.IsKeyPressed(Key.Y))
        // {
        //     
        // }
        
        // if (Input.IsKeyPressed(Key.H))
        // {
       
        // }
    }
}
