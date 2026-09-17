using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DMGStarterTemplate;

// Plain serializable DTO. Deliberately NOT a Godot Node: deriving from Node would
// serialize dozens of engine properties (including a raw NativeInstance pointer) into the save file.
//
// Enum rule for this file: every enum is stored by NAME, never by ordinal. Dictionary keys
// (achievementsUnlocked) already serialize by name; the StringEnumConverter below makes plain enum
// fields do the same. Names can be reordered freely but must never be renamed or removed once a
// build has shipped. (Integer values in older saves still load: the converter accepts both.)
public class SaveGameData
{
    public float mainVolume = .5f;
    public float musicVolume = .5f;
    public float soundVolume = .5f;
    public int currency = 0;

    // null = never chosen; the project/OS default applies until the player toggles it in Settings.
    public bool? windowed = null;
    public AchievementData achievementData = new();

    [JsonConverter(typeof(StringEnumConverter))]
    public SupportedLanguages currentLanguage = SupportedLanguages.EN;

    public AchievementProgressData achievementProgressData = new();

}
