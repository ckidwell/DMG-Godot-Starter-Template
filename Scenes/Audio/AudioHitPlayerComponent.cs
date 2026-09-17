using Godot;

namespace DMGStarterTemplate;

// Positional (2D) hit-sound player meant to be attached to an entity that takes hits.

public partial class AudioHitPlayerComponent : AudioStreamPlayer2D
{
    [Export] private AudioStream[] hitSounds;

    [Export] private bool randomizePitch = true;
    [Export] private float minPitch = .9f;
    [Export] private float maxPitch = 1.1f;

    private RandomNumberManager _random;

    public override void _Ready()
    {
        _random = GetNode<RandomNumberManager>("/root/RandomNumberManager");
    }

    public void PlayRandomHit()
    {
        if (hitSounds == null || hitSounds.Length == 0) return;

        PitchScale = randomizePitch ? _random.GetRandomNumber(minPitch, maxPitch) : 1f;

        Stream = hitSounds[_random.GetRandomIndex(hitSounds.Length)];
        Play();
    }
}
