using Godot;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace MyGame;

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

	private void OnScreenShake(float duration, float strength, float strengthDecayRate, float rampTime, float rampStrength)
	{
		shaking = true;
		StartCoroutine(ScreenShake(duration, strength, strengthDecayRate ));
	}

	public override void _Process(double delta)
	{
		//if (shaking) return; 
		CameraTrack();

		GlobalPosition = GlobalPosition.Lerp(targetPosition, (float)(1.0f - Mathf.Exp(-delta * 20)));
	}
	
	private IEnumerable ScreenShake(float duration, float strength, float shakeDecayRate)
	{
	
		var endTime = duration * 1000 +  Time.GetTicksMsec();
		
		while (endTime > Time.GetTicksMsec())
		{
			strength = float.Lerp(strength, 0, shakeDecayRate * (float)GetProcessDeltaTime());

			Offset = GetRandomOffset(strength);
			yield return  GetTree().CreateTimer(.05);
		}
		
		shaking = false;
		yield return null;
	}
	private static async Task StartCoroutine(IEnumerable objects)
	{
		var mainLoopTree = Engine.GetMainLoop();
		foreach (var _ in objects)
		{
			await mainLoopTree.ToSignal(mainLoopTree, SceneTree.SignalName.ProcessFrame);
		}
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
