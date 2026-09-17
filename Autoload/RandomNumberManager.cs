using Godot;

namespace DMGStarterTemplate;

// The one random source for the game. Every system that needs randomness (spawning, loot,
// sound variation, camera shake) should draw from here rather than GD.RandRange or its own
// RandomNumberGenerator, otherwise turning on useDeterministicSeed only makes *some* of the game
// reproducible.
public partial class RandomNumberManager : Node
{
    private readonly RandomNumberGenerator _randomNumberGenerator = new RandomNumberGenerator();

    [Export] public ulong deterministicSeed = 0;
    [Export] public bool useDeterministicSeed = false;

    // The seed actually in use this session. Log or display it so a bug seen in a random run can
    // be reproduced by setting deterministicSeed to this value.
    public ulong Seed => _randomNumberGenerator.Seed;

    public override void _Ready()
    {
        if (useDeterministicSeed)
        {
            _randomNumberGenerator.Seed = deterministicSeed;
            return;
        }

        _randomNumberGenerator.Randomize();
    }

    // Inclusive on both ends.
    public int GetRandomNumber(int min, int max)
    {
        return _randomNumberGenerator.RandiRange(min, max);
    }

    public float GetRandomNumber(float min, float max)
    {
        return _randomNumberGenerator.RandfRange(min, max);
    }
    
    public int GetRandomIndex(int count)
    {
        return count <= 0 ? -1 : _randomNumberGenerator.RandiRange(0, count - 1);
    }
}
