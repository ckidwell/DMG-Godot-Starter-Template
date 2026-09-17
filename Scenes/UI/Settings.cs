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
	private OptionButton _languageOptionButton;

	private SaveGameDataVariant saveGameData;
	private GameEvents _gameEvents;
	private MenuSystemManager _menuSystemManager;

	// The language list is rebuilt from the enum, in this order, so item index == enum index.
	private static readonly SupportedLanguages[] Languages = Enum.GetValues<SupportedLanguages>();

	public override void _EnterTree()
	{
		_gameEvents = GetNode<GameEvents>("/root/GameEvents");
		_gameEvents.SaveGameDataUpdated += OnSaveGameDataUpdated;
	}

	public override void _Ready()
	{
		_menuSystemManager = GetNode<MenuSystemManager>("/root/MenuSystemManager");

		_backButton = GetNode<TextureButton>("%BackButton");
		_backButton.Pressed += OnBackButtonPressed;

		// Checked == windowed. The label next to it stays the translated "WINDOWED_MODE_" key from
		// the scene; the check state, not the label text, shows the current mode.
		_windowedCheckButton = GetNode<CheckButton>("%WindowedCheckButton");
		_windowedCheckButton.Toggled += OnWindowedToggled;

		_languageOptionButton = GetNode<OptionButton>("%LanguageOptionButton");
		PopulateLanguageOptions();
		_languageOptionButton.ItemSelected += OnLanguageSelected;

		_mainVolumeSlider = GetNode<HSlider>("%MainVolumeSlider");
		_mainVolumeSlider.ValueChanged += OnMainVolumeValueChanged;

		_soundEffectsSlider = GetNode<HSlider>("%SFXSlider");
		_soundEffectsSlider.ValueChanged += OnEffectsValueChanged;

		_musicSlider = GetNode<HSlider>("%MusicSlider");
		_musicSlider.ValueChanged += OnMusicValueChanged;

		_mainVolumeSlider.DragEnded += OnSliderDragEnded;
		_soundEffectsSlider.DragEnded += OnSliderDragEnded;
		_musicSlider.DragEnded += OnSliderDragEnded;

		UpdateDisplay();
	}

	public override void _ExitTree()
	{
		if (_gameEvents == null) return;
		_gameEvents.SaveGameDataUpdated -= OnSaveGameDataUpdated;
	}

	public override void _Notification(int what)
	{
		// OptionButton items were added with Tr() at population time, so re-translate them when
		// the locale changes (auto-translate only covers text set in the scene).
		if (what == NotificationTranslationChanged && _languageOptionButton != null)
		{
			PopulateLanguageOptions();
		}
	}

	private void OnSaveGameDataUpdated(SaveGameDataVariant data)
	{
		saveGameData = data;
		_mainVolumeSlider?.SetValueNoSignal(data.SaveGameData.mainVolume);
		_soundEffectsSlider?.SetValueNoSignal(data.SaveGameData.soundVolume);
		_musicSlider?.SetValueNoSignal(data.SaveGameData.musicVolume);
		SelectLanguage(data.SaveGameData.currentLanguage);
	}

	private void OnBackButtonPressed()
	{
		_menuSystemManager.SetCurrentMenu(MenuType.MAIN);
	}

	private void OnWindowedToggled(bool windowed)
	{
		WindowModeHelper.SetWindowed(windowed);
		_gameEvents.EmitWindowModeChanged(windowed);
	}

	private void PopulateLanguageOptions()
	{
		var current = _languageOptionButton.Selected;

		_languageOptionButton.Clear();
		foreach (var language in Languages)
		{
			_languageOptionButton.AddItem(Tr(language.DisplayNameKey()), (int)language);
		}

		if (current >= 0) _languageOptionButton.Selected = current;
		else SelectLanguage(ProgressionManager.GetSaveGameData().currentLanguage);
	}

	private void SelectLanguage(SupportedLanguages language)
	{
		if (_languageOptionButton == null) return;
		var index = Array.IndexOf(Languages, language);
		if (index >= 0) _languageOptionButton.Selected = index;
	}

	private void OnLanguageSelected(long index)
	{
		_gameEvents.EmitPlayAudioStream(GameConstants.UI_CLICK_BUTTON);
		var language = (SupportedLanguages)_languageOptionButton.GetItemId((int)index);
		// ProgressionManager sets the locale and saves; every auto-translated label updates itself.
		_gameEvents.EmitSupportedLanguageUpdated(new SupportedLanguagesVariant(language));
	}

	private void OnMainVolumeValueChanged(double value)
	{
		var amount = (float) value;
		AudioBus.SetVolumePercent(GameConstants.MAIN_BUS, amount);
		_gameEvents.EmitMainVolume(amount);
	}
	private void OnMusicValueChanged(double value)
	{
		var amount = (float) value;
		AudioBus.SetVolumePercent(GameConstants.MUSIC_BUS, amount);
		_gameEvents.EmitMusicVolume(amount);
	}

	private void OnEffectsValueChanged(double value)
	{
		var amount = (float) value;
		AudioBus.SetVolumePercent(GameConstants.EFFECTS_BUS, amount);
		_gameEvents.EmitSoundVolume(amount);
	}

	private void OnSliderDragEnded(bool valueChanged)
	{
		if (!valueChanged) return;
		_gameEvents.EmitPlayAudioStream(GameConstants.UI_CLICK_BUTTON);
	}

	private void UpdateDisplay()
	{
		// Reflect the real window mode, whatever set it (project setting, command line, saved preference).
		_windowedCheckButton.SetPressedNoSignal(WindowModeHelper.IsWindowed);

		_soundEffectsSlider.SetValueNoSignal(AudioBus.GetVolumePercent(GameConstants.EFFECTS_BUS));
		_musicSlider.SetValueNoSignal(AudioBus.GetVolumePercent(GameConstants.MUSIC_BUS));
		_mainVolumeSlider.SetValueNoSignal(AudioBus.GetVolumePercent(GameConstants.MAIN_BUS));

		SelectLanguage(ProgressionManager.GetSaveGameData().currentLanguage);
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
