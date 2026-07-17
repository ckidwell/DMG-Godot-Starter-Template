using Godot;
using System;

namespace DMGStarterTemplate;

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

	public override void _ExitTree()
	{
		if (_gameEvents == null) return;
		_gameEvents.PlayAudioStream -= OnPlayAudioStream;
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
		if (sounds == null || sounds.Length == 0) return;

		var player = soundEventName switch
		{
			GameConstants.S_EXPLOSION => explosionPlayer,
			GameConstants.S_HIT => hitPlayer,
			GameConstants.S_XP_GEM_COLLECTED => xpPlayer,
			GameConstants.S_BULLET_FIRED => bulletPlayer,
			_ => null
		};

		if (player == null) return;
		
		player.PitchScale = randomPitch ? (float)GD.RandRange(minPitch, maxPitch) : 1f;

		var soundToPlay = GD.RandRange(0, sounds.Length - 1);
		player.Stream = sounds[soundToPlay];
		player.Play();
	}

	
	private void PlayRandomSound(AudioStream[] sounds, AudioStreamPlayer2D player,bool randomPitch = false )
	{
		if (sounds == null || sounds.Length == 0) return;
		
		player.PitchScale = randomPitch ? (float)GD.RandRange(minPitch, maxPitch) : 1f;

		var soundToPlay = GD.RandRange(0, sounds.Length - 1);
		player.Stream =  sounds[soundToPlay];
		player.Play();
	}
	private void PlaySound(AudioStream[] sounds, int indexOfSound, bool randomPitch = false)
	{
		if (sounds == null || sounds.Length == 0) return;

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
