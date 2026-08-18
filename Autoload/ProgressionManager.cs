using System;
using Godot;
using System.IO;
using Newtonsoft.Json;
using FileAccess = Godot.FileAccess;

namespace DMGStarterTemplate;

public partial class ProgressionManager : Node
{
	
	// possibly better save implementation noted in documentation
	// JSON Serialization https://docs.godotengine.org/en/stable/tutorials/io/saving_games.html#binary-serialization
	// Binary Serialization https://docs.godotengine.org/en/stable/tutorials/io/binary_serialization_api.html#doc-binary-serialization-api
	
	// generic C# saving
	//https://stackoverflow.com/questions/4266875/how-to-quickly-save-load-class-instance-to-file
	private static SaveGameData _saveGameData = new SaveGameData();
	private static string SAVE_FILE_PATH = "user://game_template.save";
	
	private GameEvents _gameEvents;

	// Volume sliders fire ValueChanged many times per drag; coalesce those into a single
	// write shortly after the last change instead of writing the file on every frame.
	private Timer _saveDebounceTimer;
	private bool _pendingSave;
	private const double SaveDebounceSeconds = 0.5;


	public override void _Ready()
	{
		SAVE_FILE_PATH = ProjectSettings.GlobalizePath(SAVE_FILE_PATH);
		
		_gameEvents = GetNode<GameEvents>("/root/GameEvents");
		_gameEvents.SupportedLanguageUpdated += OnSupportedLanguageUpdated;
		_gameEvents.Died += OnDied;
		_gameEvents.CurrencyPickedUp += OnCurrencyPickup;
		_gameEvents.SoundVolume += OnSoundVolumeChanged;
		_gameEvents.MusicVolume += OnMusicVolumeChanged;
		_gameEvents.MainVolume += OnMainVolumeChanged;

		_saveDebounceTimer = new Timer { OneShot = true, WaitTime = SaveDebounceSeconds };
		AddChild(_saveDebounceTimer);
		_saveDebounceTimer.Timeout += OnSaveDebounceTimeout;

		LoadSaveDataFile();
		
		// AchievementUnlocked already no-ops if it's unknown or already unlocked.
		AchievementUnlocked(Achievements.WELCOME_FIRST_TIME);

		ApplyLoadedSettings();
	}

	// Push the just-loaded save data back out so it actually takes effect for the session.
	// Without this the audio buses stay at their defaults, and the first visit to the Settings
	// menu overwrites the saved volumes with those defaults, so they never persist across sessions.
	private void ApplyLoadedSettings()
	{
		AudioBus.SetVolumePercent(GameConstants.MAIN_BUS, _saveGameData.mainVolume);
		AudioBus.SetVolumePercent(GameConstants.MUSIC_BUS, _saveGameData.musicVolume);
		AudioBus.SetVolumePercent(GameConstants.EFFECTS_BUS, _saveGameData.soundVolume);

		TranslationServer.SetLocale(_saveGameData.currentLanguage.ToLocale());

		// Broadcast so any menu or card already listening refreshes from the loaded data.
		_gameEvents.EmitSaveGameDataUpdated(new SaveGameDataVariant(_saveGameData));
	}

	public override void _ExitTree()
	{
		// Persist any change still waiting in the debounce window before we shut down.
		FlushPendingSave();

		if (_gameEvents == null) return;
		_gameEvents.SupportedLanguageUpdated -= OnSupportedLanguageUpdated;
		_gameEvents.Died -= OnDied;
		_gameEvents.CurrencyPickedUp -= OnCurrencyPickup;
		_gameEvents.SoundVolume -= OnSoundVolumeChanged;
		_gameEvents.MusicVolume -= OnMusicVolumeChanged;
		_gameEvents.MainVolume -= OnMainVolumeChanged;
	}

	private void OnMainVolumeChanged(float amount)
	{
		_saveGameData.mainVolume = amount;
		RequestSave();
	}

	private void OnMusicVolumeChanged(float amount)
	{
		_saveGameData.musicVolume = amount;
		RequestSave();
	}

	private void OnSoundVolumeChanged(float amount)
	{
		_saveGameData.soundVolume = amount;
		RequestSave();
	}

	private void OnSupportedLanguageUpdated(SupportedLanguagesVariant lang)
	{
		_saveGameData.currentLanguage = lang.sl;
		TranslationServer.SetLocale(lang.sl.ToLocale());
		WriteSaveDataFile();
	}



	public void AchievementUnlocked(Achievements achievement)
	{
		if (!_saveGameData.achievementData.achievementsUnlocked.TryGetValue(achievement, out var ach)) return;

		if (ach) return;
		
		_saveGameData.achievementData.achievementsUnlocked[achievement] = true;
		WriteSaveDataFile();
		_gameEvents.EmitAchievementEarned(new AchievementDescriptionVariant(AchievementDescription.GetDescriptionForAchievement(achievement)));

		// Notify any live UI (e.g. an open achievements menu) that the saved data changed.
		_gameEvents.EmitSaveGameDataUpdated(new SaveGameDataVariant(_saveGameData));
	}


	private void OnDied(Vector2 arg1, bool isPlayer, double experienceDropPercentage)
	{
		if (!isPlayer)
		{
			_saveGameData.achievementProgressData.enemiesKilled += 1;

			return;
		}

		AchievementUnlocked(Achievements.DIED_FIRST_TIME);


		var killed = _saveGameData.achievementProgressData.enemiesKilled;

		// Need these individual IF statements not a else if or switch because otherwise say its your
		// first game and you get 101 kills you would only get the achievement for 100 and never be able to get the KILL_1
		if (killed > 0)
		{
			AchievementUnlocked(Achievements.KILL_1);
		}
		
		_gameEvents.EmitCurrencyUpdated(_saveGameData.currency);
		WriteSaveDataFile();
	}

	public static SaveGameData GetSaveGameData()
	{
		return _saveGameData;
	}
	private static void PrintSaveGameDataChanges(SaveGameData currentData, string filePath)
	{
		if (!FileAccess.FileExists(filePath))
		{
			GD.Print($"No save game data found when attempting to write file");
			return;
		} 

		var previousData = ReadFromJsonFile<SaveGameData>(filePath);

		var currentDataJson = JsonConvert.SerializeObject(currentData);
		var previousDataJson = JsonConvert.SerializeObject(previousData);

		var hasChanged = !currentDataJson.Equals(previousDataJson);

		if (hasChanged)
		{
			GD.Print($"Changes detected in saved game data:\n{currentDataJson}\nvs.\n{previousDataJson}");
		}

	}
	public void LoadSaveDataFile()
	{
		if (!FileAccess.FileExists(SAVE_FILE_PATH))
		{
			SeedData();
			return;
		}
		
		try
		{
			var saveFile = ReadFromJsonFile<SaveGameData>(SAVE_FILE_PATH);

			// DeserializeObject returns null for empty or "null" file contents.
			if (saveFile == null)
			{
				GD.PushWarning("Save file was empty or invalid; starting from defaults.");
				SeedData();
				return;
			}

			_saveGameData = saveFile;
		}
		catch (Exception e)
		{
			// A corrupt save must never crash startup. Keep the default _saveGameData and carry on.
			GD.PushError($"Failed to load save file, starting from defaults: {e.Message}");
			SeedData();
		}
		//
		// // this will seed the data on first time use
		// if (_saveGameData.upgradesSaveData.Count != 0) return;
		//
		// foreach (var metaUpgradeData in MetaUpgradeList.GetMetaUpgrades())
		// {
		// 	_saveGameData.upgradesSaveData.Add(metaUpgradeData.id,metaUpgradeData);
		// }


	}

	private void SeedData()
	{
		
		// if (_saveGameData.upgradesSaveData.Count != 0) return;
		//
		// //only seeding MetaUpgrade's because its the only item as of this writing that is not complete when created new()
		// foreach (var metaUpgradeData in MetaUpgradeList.GetMetaUpgrades())
		// {
		// 	_saveGameData.upgradesSaveData.Add(metaUpgradeData.id,metaUpgradeData);    
		// }
	}
	// Debounced save: (re)start the countdown so the file is written once changes stop for
	// SaveDebounceSeconds. Prevents the per-frame write storm caused by dragging a volume slider.
	private void RequestSave()
	{
		_pendingSave = true;
		_saveDebounceTimer.Start();
	}

	private void OnSaveDebounceTimeout()
	{
		if (_pendingSave) WriteSaveDataFile();
	}

	private void FlushPendingSave()
	{
		if (_pendingSave) WriteSaveDataFile();
	}

	public void WriteSaveDataFile()
	{
		// Any pending debounced write is now satisfied by this immediate write.
		_pendingSave = false;
		_saveDebounceTimer?.Stop();
		WriteToJsonFile(SAVE_FILE_PATH, _saveGameData);
	}
	
	
	private void OnCurrencyPickup(int amount)
	{
		_saveGameData.currency += amount;
		_gameEvents.EmitCurrencyUpdated(_saveGameData.currency);
	}

	#region JSON_SAVER

	/// <summary>
	/// Serializes the given object instance to JSON and writes it atomically.
	/// <para>Object type must have a parameterless constructor.</para>
	/// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
	/// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [JsonIgnore] attribute.</para>
	/// <para>The data is written to a temporary file first and then swapped into place, so an interrupted
	/// write (crash, power loss, full disk) can never leave a truncated or corrupt save file behind.</para>
	/// </summary>
	/// <typeparam name="T">The type of object being written to the file.</typeparam>
	/// <param name="filePath">The file path to write the object instance to.</param>
	/// <param name="objectToWrite">The object instance to write to the file.</param>
	private static void WriteToJsonFile<T>(string filePath, T objectToWrite) where T : new()
	{
		var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite);

		var tempFilePath = filePath + ".tmp";
		File.WriteAllText(tempFilePath, contentsToWriteToFile);

		if (File.Exists(filePath))
		{
			// File.Replace is atomic on the same volume and keeps the original intact until the swap succeeds.
			File.Replace(tempFilePath, filePath, null);
		}
		else
		{
			File.Move(tempFilePath, filePath);
		}
	}

	/// <summary>
	/// Reads an object instance from an Json file.
	/// <para>Object type must have a parameterless constructor.</para>
	/// </summary>
	/// <typeparam name="T">The type of object to read from the file.</typeparam>
	/// <param name="filePath">The file path to read the object instance from.</param>
	/// <returns>Returns a new instance of the object read from the Json file.</returns>
	private static T ReadFromJsonFile<T>(string filePath) where T : new()
	{
		TextReader reader = null;
		try
		{
			reader = new StreamReader(filePath);
			var fileContents = reader.ReadToEnd();
			return JsonConvert.DeserializeObject<T>(fileContents);
		}
		finally
		{
			if (reader != null)
				reader.Close();
		}
	}

	#endregion
}
