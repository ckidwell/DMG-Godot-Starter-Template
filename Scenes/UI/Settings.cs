using Godot;
using System;
using System.Threading.Tasks;

public partial class Settings : CanvasLayer
{
	[Export] private Control _visualControlParent;
    private Button _windowModeButton;
    private CheckButton _windowedCheckButton;
	private TextureButton _backButton;
	private HSlider _mainVolumeSlider;
	private HSlider _soundEffectsSlider;
	private HSlider _musicSlider;
	private Label _windowModeLabel;

	private SaveGameDataVariant saveGameData;
	private GameEvents _gameEvents;
	private MenuSystemManager _menuSystemManager;
	
	bool fullscreen = true;

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



	private void OnSaveGameDataUpdated(SaveGameDataVariant data)
	{
		saveGameData = data;
		SessionConfigurationManager.SetBusVolumePercent(GameConstants.MUSIC_BUS, (float)data.SaveGameData.musicVolume);
		SessionConfigurationManager.SetBusVolumePercent(GameConstants.EFFECTS_BUS, (float)data.SaveGameData.soundVolume);
	}

	private void OnBackButtonPressed()
	{
		_gameEvents.EmitPlayAudioStream(GameConstants.UI_CLICK_BUTTON);
		_gameEvents.EmitPlayAudioStream(GameConstants.UI_CLICK_BUTTON);
		_menuSystemManager.SetCurrentMenu(MenuType.MAIN);
	}

	private void OnWindowedCheckButtonPressed()
	{
		GD.Print("Windowed check button pressed");
		_gameEvents.EmitPlayAudioStream(GameConstants.UI_CLICK_BUTTON);
		
		var mode = DisplayServer.WindowGetMode();
		
		if (mode != DisplayServer.WindowGetMode())
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen); 
		}
		else
		{
			DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		}
	}

	
	private void OnMainVolumeValueChanged(double value)
	{
		_gameEvents.EmitPlayAudioStream(GameConstants.UI_CLICK_BUTTON);
		var amount = (float) value;
		SessionConfigurationManager.SetBusVolumePercent(GameConstants.MAIN_BUS, amount);
		_gameEvents.EmitMainVolume(amount);
	}
	private void OnMusicValueChanged(double value)
	{
		_gameEvents.EmitPlayAudioStream(GameConstants.UI_CLICK_BUTTON);
		var amount = (float) value;
		SessionConfigurationManager.SetBusVolumePercent(GameConstants.MUSIC_BUS, amount);
		_gameEvents.EmitMusicVolume(amount);
	}

	private void OnEffectsValueChanged(double value)
	{
		_gameEvents.EmitPlayAudioStream(GameConstants.UI_CLICK_BUTTON);
		var amount = (float) value;
		SessionConfigurationManager.SetBusVolumePercent(GameConstants.EFFECTS_BUS, amount);
		_gameEvents.EmitSoundVolume(amount);
	}

	private float GetBusVolumePercent(string busName)
	{
		// all this is needed because DB's are not linear apparently
		// so we have to convert DB's to a percentage to use a volume slider
		var bus_index = AudioServer.GetBusIndex(busName);
		var volume_db = AudioServer.GetBusVolumeDb(bus_index);

		return Mathf.DbToLinear(volume_db);
	}
	
	private void UpdateDisplay()
	{
		_windowModeLabel.Text = "SET WINDOWED";
		
		if(DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Windowed)
			_windowModeLabel.Text = "SET FULLSCREEN";
		
		_soundEffectsSlider.Value = GetBusVolumePercent(GameConstants.EFFECTS_BUS);
		_musicSlider.Value = GetBusVolumePercent(GameConstants.MUSIC_BUS);
		_mainVolumeSlider.Value = GetBusVolumePercent(GameConstants.MAIN_BUS);
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
