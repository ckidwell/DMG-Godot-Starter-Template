using Godot;

namespace DMGStarterTemplate;

// Camera2D that follows the node in the "player" group and shakes on GameEvents.ScreenShake.
// Opt-in: drop this scene/script onto the Camera2D in your gameplay scene.
public partial class GameCameraShake : Camera2D
{
	private Vector2 targetPosition = Vector2.Zero;
	private GameEvents _gameEvents;
	private RandomNumberManager _random;

	private Node2D _player;

	#region CameraShakeProperties

	[Export] private float noiseShakeSpeed = 30.0f;
	// How quickly the camera catches up with the player: higher is snappier.
	[Export] private float followSharpness = 20.0f;
	private FastNoiseLite noise;
	private float noise_value = 0.0f;

	// Shake timing is driven by process delta (a countdown), not by wall-clock time, so it
	// pauses with the tree and respects Engine.TimeScale.
	private float shakeTimeLeft = 0.0f;
	private float currentShakeStrength = 0.0f;
	private float currentShakeDecayRate = 0.0f;

	#endregion

	public override void _Ready()
	{
		_random = GetNode<RandomNumberManager>("/root/RandomNumberManager");
		_gameEvents = GetNode<GameEvents>("/root/GameEvents");

		noise = new FastNoiseLite();
		noise.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;

		noise.Seed = _random.GetRandomNumber(int.MinValue, int.MaxValue);

		_gameEvents.ScreenShake += OnScreenShake;
		MakeCurrent();
	}

	public override void _ExitTree()
	{
		if (_gameEvents == null) return;
		_gameEvents.ScreenShake -= OnScreenShake;
	}

	private void OnScreenShake(float duration, float strength, float strengthDecayRate)
	{
		shakeTimeLeft = duration;
		currentShakeStrength = strength;
		currentShakeDecayRate = strengthDecayRate;
	}

	public override void _Process(double delta)
	{
		var dt = (float)delta;

		CameraTrack();

		// Exponential smoothing: frame-rate independent, unlike Lerp with a factor of k*delta.
		GlobalPosition = GlobalPosition.Lerp(targetPosition, 1.0f - Mathf.Exp(-dt * followSharpness));

		UpdateShake(dt);
	}

	private void UpdateShake(float dt)
	{
		if (shakeTimeLeft <= 0.0f) return;

		shakeTimeLeft -= dt;
		if (shakeTimeLeft <= 0.0f)
		{
			Offset = Vector2.Zero;
			return;
		}

		// True exponential decay: the same curve at 30 fps and 144 fps.
		currentShakeStrength *= Mathf.Exp(-currentShakeDecayRate * dt);

		noise_value += dt * noiseShakeSpeed;
		Offset = new Vector2(
			noise.GetNoise2D(1, noise_value),
			noise.GetNoise2D(100, noise_value)
		) * currentShakeStrength;
	}

	private void CameraTrack()
	{
		if (!IsInstanceValid(_player))
		{
			_player = GetTree().GetFirstNodeInGroup("player") as Node2D;
			if (_player == null) return;
		}

		targetPosition = _player.GlobalPosition;
	}
}
