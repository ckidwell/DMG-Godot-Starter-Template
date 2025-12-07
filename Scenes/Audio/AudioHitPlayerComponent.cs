using Godot;
using System;

public partial class AudioHitPlayerComponent : AudioStreamPlayer2D
{
    [Export] private AudioStream[] hitSounds;
	
    [Export] private bool randomizePitch = true;
    [Export] private float minPitch = .9f;
    [Export] private float maxPitch = 1.1f;
	
	
    public void PlayRandomHit()
    {
        if (hitSounds.Length == 0 || hitSounds == null) return;

        if (randomizePitch)
        {
            PitchScale = (float)GD.RandRange(minPitch, maxPitch);
        }
        else
        {
            PitchScale = 1f;
        }
			
		
        var soundToPlay =GD.RandRange(0, hitSounds.Length - 1);
        Stream =  hitSounds[soundToPlay];
        Play();
    }
}
