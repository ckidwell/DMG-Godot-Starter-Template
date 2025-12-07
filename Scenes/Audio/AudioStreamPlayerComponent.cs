using Godot;
using System;

public partial class AudioStreamPlayerComponent : AudioStreamPlayer
{
    [Export] private AudioStream[] xpSounds;
	[Export] private AudioStream[] gunfireSounds;
	[Export] private AudioStream[] explosionSounds;
	[Export] private AudioStream[] hitSounds;
	[Export] private AudioStream[] UISounds;
	[Export] private AudioStream[] coinSounds;
	[Export] private AudioStream[] healthCollectedSounds;

	[Export] private bool randomizeClickSoundsPitch = true;
	[Export] private float minPitch = .9f;
	[Export] private float maxPitch = 1.1f;

	[Export] private AudioStreamPlayer2D hitPlayer;
	[Export] private AudioStreamPlayer2D bulletPlayer;
	[Export] private AudioStreamPlayer2D explosionPlayer;
	[Export] private AudioStreamPlayer2D xpPlayer;
	
	private GameEvents _gameEvents;

	public override void _Ready()
	{
		_gameEvents = GetNode<GameEvents>("/root/GameEvents");
		_gameEvents.PlayAudioStream += OnPlayAudioStream;
	}

	private void OnPlayAudioStream(string soundEventName)
	{
		switch (soundEventName)
		{
			case GameConstants.S_HIT:
				PlayRandomSoundForStreamPlayer(hitSounds, true,soundEventName);
				return;
			case GameConstants.S_XP_GEM_COLLECTED:
				PlayRandomSoundForStreamPlayer(xpSounds, true,soundEventName);
				return;
			case GameConstants.S_BULLET_FIRED:
				PlayRandomSound(gunfireSounds, bulletPlayer, true);
				return;
			case GameConstants.S_COIN_COLLECTED:
				PlayRandomSound(coinSounds, xpPlayer,true);
				return;
			case GameConstants.S_HEALTH_COLLECTED:
				PlayRandomSound(healthCollectedSounds, xpPlayer,true);
				return;
			case GameConstants.UI_CLICK_BUTTON:
				PlayRandomSound(UISounds, hitPlayer,randomizeClickSoundsPitch);
				return;
		}
	}

	private void PlayRandomSoundForStreamPlayer(AudioStream[] sounds, bool randomPitch, string soundEventName)
	{
		if (sounds.Length == 0 || sounds == null) return;

		if (randomPitch)
		{
			PitchScale = (float)GD.RandRange(minPitch, maxPitch);
		}
		else
		{
			PitchScale = 1f;
		}
		var soundToPlay = GD.RandRange(0, sounds.Length - 1);

		switch (soundEventName)
		{
			case GameConstants.S_EXPLOSION:
				explosionPlayer.Stream = sounds[soundToPlay];
				explosionPlayer.Play();
				return;
			case GameConstants.S_HIT:
				hitPlayer.Stream = sounds[soundToPlay];
				hitPlayer.Play();
				return;
			case GameConstants.S_XP_GEM_COLLECTED:
				xpPlayer.Stream = sounds[soundToPlay];
				xpPlayer.Play();
				return;
			case GameConstants.S_BULLET_FIRED:
				bulletPlayer.Stream = sounds[soundToPlay];
				bulletPlayer.Play();
				return;
		}
	}

	
	private void PlayRandomSound(AudioStream[] sounds, AudioStreamPlayer2D player,bool randomPitch = false )
	{
		if (sounds.Length == 0 || sounds == null) return;

		if (randomPitch)
		{
			PitchScale = (float)GD.RandRange(minPitch, maxPitch);
		}
		else
		{
			PitchScale = 1f;
		}
		
		var soundToPlay = GD.RandRange(0, sounds.Length - 1);
		player.Stream =  sounds[soundToPlay];
		player.Play();
	}
	private void PlaySound(AudioStream[] sounds, int indexOfSound, bool randomPitch = false)
	{
		if (sounds.Length == 0 || sounds == null) return;

		if (randomPitch)
		{
			PitchScale = (float)GD.RandRange(minPitch, maxPitch);
		}
		else
		{
			PitchScale = 1f;
		}
	
		Stream =  sounds[indexOfSound];
		Play();
	}
	private void DelayedQueueFree()
	{
		Owner.QueueFree();
	}
}
