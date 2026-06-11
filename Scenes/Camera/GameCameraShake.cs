using Godot;
using System;


public partial class GameCameraShake : Camera2D
{
    private Vector2 targetPosition = Vector2.Zero;
	private GameEvents _gameEvents;
	private RandomNumberGenerator _randomNumberGenerator;
	
	#region CameraShakeProperties
	
	[Export] private float shakeStrength = 80.0f;
	[Export]private float noiseShakeSpeed = 30.0f;
	private FastNoiseLite noise;
	private float shake_decay_rate = 0.0f;
	private float noise_value = 0.0f;
	private bool shaking = false;

	private double shakeEndTimeMsec = 0.0;
	private float currentShakeStrength = 0.0f;
	private float currentShakeDecayRate = 0.0f;

	#endregion
	
	
	
	public override void _Ready()
	{
		_randomNumberGenerator = new RandomNumberGenerator();
		_randomNumberGenerator.Randomize();
		
		_gameEvents = GetNode<GameEvents>("/root/GameEvents");
		
		noise = new FastNoiseLite();
		noise.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;
		noise.Seed = (int)_randomNumberGenerator.Randi();
		
		_gameEvents.ScreenShake += OnScreenShake;
		MakeCurrent();
	}

	public override void _ExitTree()
	{
		if (_gameEvents == null) return;
		_gameEvents.ScreenShake -= OnScreenShake;
	}

	private void OnScreenShake(float duration, float strength, float strengthDecayRate, float rampTime, float rampStrength)
	{
		shaking = true;
		currentShakeStrength = strength;
		currentShakeDecayRate = strengthDecayRate;
		shakeEndTimeMsec = duration * 1000 + Time.GetTicksMsec();
	}

	public override void _Process(double delta)
	{
		CameraTrack();

		GlobalPosition = GlobalPosition.Lerp(targetPosition, (float)(1.0f - Mathf.Exp(-delta * 20)));

		UpdateShake(delta);
	}

	private void UpdateShake(double delta)
	{
		if (!shaking) return;

		if (Time.GetTicksMsec() >= shakeEndTimeMsec)
		{
			shaking = false;
			Offset = Vector2.Zero;
			return;
		}

		currentShakeStrength = float.Lerp(currentShakeStrength, 0, currentShakeDecayRate * (float)delta);
		Offset = GetRandomOffset(currentShakeStrength);
	}

	private Vector2 GetRandomOffset(float strength)
	{
		noise_value += (float)GetProcessDeltaTime() * noiseShakeSpeed;
		
		return new Vector2(
			noise.GetNoise2D(1, noise_value) * strength,
			noise.GetNoise2D(100, noise_value) * strength
			);
	}
	
	private void CameraTrack()
	{
		var playerNodes = GetTree().GetNodesInGroup("player");

		if (playerNodes.Count <= 0) return;
		var player = playerNodes[0] as Node2D;
		
		targetPosition = player.GlobalPosition;
	}
}
