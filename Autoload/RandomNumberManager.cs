using Godot;
using System;

public partial class RandomNumberManager : Node
{
    private RandomNumberGenerator _randomNumberGenerator = new RandomNumberGenerator();
    [Export] public ulong deterministicSeed = 0;
    [Export] public bool useDeterministicSeed = false;
    public override void _Ready()
    {
        base._Ready();
        if (useDeterministicSeed)
        {
            _randomNumberGenerator.Seed = deterministicSeed;
            return;
        }
        
        _randomNumberGenerator.Seed = (ulong)DateTime.Now.Ticks;    
        
    }

    public int GetRandomNumber(int min, int max)
    {
        return _randomNumberGenerator.RandiRange(min, max);
    }
    public float GetRandomNumber(float min, float max)
    {
        return _randomNumberGenerator.RandfRange(min, max);
    }
    
}
