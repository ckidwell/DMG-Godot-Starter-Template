using Godot;
using System;

namespace MyGame;

public partial class SoundButton : Button
{
	private GameEvents _gameEvents;
	
	public override void _Ready()
	{
		_gameEvents = GetNode<GameEvents>("/root/GameEvents");
		Pressed += OnPressed;
	}

	private void OnPressed()
	{
		_gameEvents.EmitPlayAudioStream(GameConstants.UI_CLICK_BUTTON);
	}
}
