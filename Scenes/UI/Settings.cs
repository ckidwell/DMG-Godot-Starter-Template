using Godot;
using System;

namespace DMGStarterTemplate;

public partial class Settings : CanvasLayer
{
	[Export] private Control _visualControlParent;
    private CheckButton _windowedCheckButton;
	private TextureButton _backButton;
	private HSlider _mainVolumeSlider;
	private HSlider _soundEffectsSlider;
	private HSlider _musicSlider;
	private Label _windowModeLabel;

	private SaveGameDataVariant saveGameData;
	private GameEvents _gameEvents;
	private MenuSystemManager _menuSystemManager;

	public override void _Ready()
	{
		
		_menuSystemManager = GetNode<MenuSystemManager>("/root/MenuSystemManager");
			
		_gameEvents = GetNode<GameEvents>("/root/GameEvents");
		_gameEvents.SaveGameDataUpdated += OnSaveGameDataUpdated;
		
		_backButton = GetNode<TextureButton>("%BackButton");
		_backButton.Pressed += OnBackButtonPressed;
		
		_windowModeLabel = GetNode<Label>("%WindowModeLabel");
		
		_windowedCheckButton = GetNode<CheckButton>("%WindowedCheckButton");
		_windowedCheckButton.Pressed += OnWindowedCheckButtonPressed;
		
		_mainVolumeSlider = GetNode<HSlider>("%MainVolumeSlider");
		_mainVolumeSlider.ValueChanged += OnMainVolumeValueChanged;
		
		_soundEffectsSlider = GetNode<HSlider>("%SFXSlider");
		_soundEffectsSlider.ValueChanged += OnEffectsValueChanged;
		
		_musicSlider = GetNode<HSlider>("%MusicSlider");
		_musicSlider.ValueChanged += OnMusicValueChanged;
		
		UpdateDisplay();
	}

	public override void _ExitTree()
	{
		if (_gameEvents == null) return;
		_gameEvents.SaveGameDataUpdated -= OnSaveGameDataUpdated;
	}

	private void OnSaveGameDataUpdated(SaveGameDataVariant data)
	{
		saveGameData = data;
		AudioBus.SetVolumePercent(GameConstants.MUSIC_BUS, data.SaveGameData.musicVolume);
		AudioBus.SetVolumePercent(GameConstants.EFFECTS_BUS, data.SaveGameData.soundVolume);
	}

	private void OnBackButtonPressed()
	{
		_gameEvents.EmitPlayAudioStream(GameConstants.UI_CLICK_BUTTON);
		_menuSystemManager.SetCurrentMenu(MenuType.MAIN);
	}

	private void OnWindowedCheckButtonPressed()
	{
		_gameEvents.EmitPlayAudioStream(GameConstants.UI_CLICK_BUTTON);

		var isWindowed = DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Windowed;

		if (isWindowed)
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		}
		else
		{
			DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		}

		UpdateWindowModeLabel();
	}

	
	private void OnMainVolumeValueChanged(double value)
	{
		_gameEvents.EmitPlayAudioStream(GameConstants.UI_CLICK_BUTTON);
		var amount = (float) value;
		AudioBus.SetVolumePercent(GameConstants.MAIN_BUS, amount);
		_gameEvents.EmitMainVolume(amount);
	}
	private void OnMusicValueChanged(double value)
	{
		_gameEvents.EmitPlayAudioStream(GameConstants.UI_CLICK_BUTTON);
		var amount = (float) value;
		AudioBus.SetVolumePercent(GameConstants.MUSIC_BUS, amount);
		_gameEvents.EmitMusicVolume(amount);
	}

	private void OnEffectsValueChanged(double value)
	{
		_gameEvents.EmitPlayAudioStream(GameConstants.UI_CLICK_BUTTON);
		var amount = (float) value;
		AudioBus.SetVolumePercent(GameConstants.EFFECTS_BUS, amount);
		_gameEvents.EmitSoundVolume(amount);
	}

	private void UpdateWindowModeLabel()
	{
		// Label reads as the action the button will perform, so it shows the opposite of the current mode.
		_windowModeLabel.Text =
			DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Windowed
				? "SET FULLSCREEN"
				: "SET WINDOWED";
	}

	private void UpdateDisplay()
	{
		UpdateWindowModeLabel();

		_soundEffectsSlider.Value = AudioBus.GetVolumePercent(GameConstants.EFFECTS_BUS);
		_musicSlider.Value = AudioBus.GetVolumePercent(GameConstants.MUSIC_BUS);
		_mainVolumeSlider.Value = AudioBus.GetVolumePercent(GameConstants.MAIN_BUS);
	}

	public void HideVisuals()
	{
		_visualControlParent.Visible = false;
	}

	public void ShowVisuals()
	{
		_visualControlParent.Visible = true;
	}
}
