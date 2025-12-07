using System;
using Godot;
using System.IO;
using Newtonsoft.Json;
using FileAccess = Godot.FileAccess;


public partial class SaveGameManager : Node
{
    // possibly better save implementation noted in documentation
	// JSON Serialization https://docs.godotengine.org/en/stable/tutorials/io/saving_games.html#binary-serialization
	// Binary Serialization https://docs.godotengine.org/en/stable/tutorials/io/binary_serialization_api.html#doc-binary-serialization-api
	
	// generic C# saving
	//https://stackoverflow.com/questions/4266875/how-to-quickly-save-load-class-instance-to-file
	private static SaveGameData _saveGameData = new SaveGameData();
	private static string SAVE_FILE_PATH = "user://godot_templategame.save";
	
	private GameEvents _gameEvents;
	
	public override void _Ready()
	{
		SAVE_FILE_PATH = ProjectSettings.GlobalizePath("user://godot_templategame.save");
		
		_gameEvents = GetNode<GameEvents>("/root/GameEvents");
		_gameEvents.SupportedLanguageUpdated += OnSupportedLanguageUpdated;
		_gameEvents.Died += OnDied;
		_gameEvents.CurrencyPickedUp += OnCurrencyPickup;
		_gameEvents.SoundVolume += OnSoundVolumeChanged;
		_gameEvents.MusicVolume += OnMusicVolumeChanged;
	
		LoadSaveDataFile();
		
		if (_saveGameData.achievementData.achievementsUnlocked.TryGetValue(Achievements.WELCOME_FIRST_TIME, out var ach))
		{
			AchievementUnlocked(Achievements.WELCOME_FIRST_TIME);
		};
	}

	private void OnMusicVolumeChanged(float amount)
	{
		_saveGameData.musicVolume = amount;
		WriteSaveDataFile();
	}

	private void OnSoundVolumeChanged(float amount)
	{
		_saveGameData.soundVolume = amount;
		WriteSaveDataFile();
	}

	private void OnSupportedLanguageUpdated(SupportedLanguagesVariant lang)
	{
		_saveGameData.currentLanguage = lang.sl;
		WriteSaveDataFile();
	}

	private void OnCurrencyPickup(int amount)
	{
		_saveGameData.currency += amount;
		_gameEvents.EmitCurrencyUpdated(_saveGameData.currency);
	}
	public void AchievementUnlocked(Achievements achievement)
	{
		if (!_saveGameData.achievementData.achievementsUnlocked.TryGetValue(achievement, out var ach)) return;

		if (ach) return;
		
		_saveGameData.achievementData.achievementsUnlocked[achievement] = true;
		WriteSaveDataFile();
		_gameEvents.EmitAchievementEarned(new AchievementDescriptionVariant(AchievementDescription.GetDescriptionForAchievement(achievement)));
	}


	private void OnDied(Vector2 arg1, bool isPlayer, double experienceDropPercentage)
	{
		if (!isPlayer)
		{
			_saveGameData.achievementProgressData.enemiesKilled += 1;
			
			return;
		};

		if (_saveGameData.achievementData.achievementsUnlocked.TryGetValue(Achievements.DIED_FIRST_TIME, out var ach))
		{
			AchievementUnlocked(Achievements.DIED_FIRST_TIME);
		};
		
		var killed = _saveGameData.achievementProgressData.enemiesKilled;

		// Need these individual IF statements not a else if or switch because otherwise say its your
		// first game and you get 101 kills you would only get the achievement for 100 and never be able to get the KILL_1
		if (killed > 0)
		{
			AchievementUnlocked(Achievements.KILL_1);
		}

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
		
		var saveFile = ReadFromJsonFile<SaveGameData>(SAVE_FILE_PATH);
		_saveGameData = saveFile;
		
	}

	private void SeedData()
	{
		// seed data if necessary to a fresh save game file
		
		
		// example below from other game
		
		// if (_saveGameData.upgradesSaveData.Count != 0) return;
		//
		// //only seeding MetaUpgrade's because its the only item as of this writing that is not complete when created new()
		// foreach (var metaUpgradeData in MetaUpgradeList.GetMetaUpgrades())
		// {
		// 	_saveGameData.upgradesSaveData.Add(metaUpgradeData.id,metaUpgradeData);    
		// }
	}
	public void WriteSaveDataFile()
	{
		WriteToJsonFile(SAVE_FILE_PATH, _saveGameData);
	}



	


	#region JSON_SAVER

	/// <summary>
	/// Writes the given object instance to a Json file.
	/// <para>Object type must have a parameterless constructor.</para>
	/// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
	/// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [JsonIgnore] attribute.</para>
	/// </summary>
	/// <typeparam name="T">The type of object being written to the file.</typeparam>
	/// <param name="filePath">The file path to write the object instance to.</param>
	/// <param name="objectToWrite">The object instance to write to the file.</param>
	/// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
	private static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
	{
		TextWriter writer = null;
		try
		{
			var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite);
			writer = new StreamWriter(filePath, append);
			writer.Write(contentsToWriteToFile);
		}
		finally
		{
			if (writer != null)
				writer.Close();
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
