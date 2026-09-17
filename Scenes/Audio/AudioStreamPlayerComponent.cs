using Godot;

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

	// Maximum number of simultaneous voices per player before the oldest is dropped.
	[Export] private int polyphony = 16;

	[Export] private AudioStreamPlayer uiPlayer;

	private GameEvents _gameEvents;

	public override void _Ready()
	{
		Stream = new AudioStreamPolyphonic { Polyphony = polyphony };

		if (uiPlayer == null)
		{
			GD.PushWarning($"{Name}: uiPlayer is not assigned; UI sounds will play through the world SFX player.");
			uiPlayer = this;
		}
		else
		{
			uiPlayer.Stream = new AudioStreamPolyphonic { Polyphony = polyphony };
		}

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
			case GameConstants.S_EXPLOSION:
				PlayRandomSound(explosionSounds, this, true);
				return;
			case GameConstants.S_HIT:
				PlayRandomSound(hitSounds, this, true);
				return;
			case GameConstants.S_XP_GEM_COLLECTED:
				PlayRandomSound(xpSounds, this, true);
				return;
			case GameConstants.S_BULLET_FIRED:
				PlayRandomSound(gunfireSounds, this, true);
				return;
			case GameConstants.S_COIN_COLLECTED:
				PlayRandomSound(coinSounds, this, true);
				return;
			case GameConstants.S_HEALTH_COLLECTED:
				PlayRandomSound(healthCollectedSounds, this, true);
				return;
			case GameConstants.UI_CLICK_BUTTON:
				PlayRandomSound(UISounds, uiPlayer, randomizeClickSoundsPitch);
				return;
		}
	}

	private void PlayRandomSound(AudioStream[] sounds, AudioStreamPlayer player, bool randomPitch)
	{
		if (sounds == null || sounds.Length == 0) return;

		var playback = GetPolyphonicPlayback(player);
		if (playback == null) return;

		var pitch = randomPitch ? (float)GD.RandRange(minPitch, maxPitch) : 1f;
		var sound = sounds[GD.RandRange(0, sounds.Length - 1)];

		playback.PlayStream(sound, pitchScale: pitch);
	}
	
	private static AudioStreamPlaybackPolyphonic GetPolyphonicPlayback(AudioStreamPlayer player)
	{
		if (!player.Playing) player.Play();
		return player.GetStreamPlayback() as AudioStreamPlaybackPolyphonic;
	}
}
