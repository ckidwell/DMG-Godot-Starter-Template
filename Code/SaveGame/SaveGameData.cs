namespace DMGStarterTemplate;

// Plain serializable DTO. Deliberately NOT a Godot Node: deriving from Node would
// serialize dozens of engine properties (including a raw NativeInstance pointer) into the save file.
public class SaveGameData
{
    public float mainVolume = .5f;
    public float musicVolume = .5f;
    public float soundVolume = .5f;
    public int currency = 0;
    public AchievementData achievementData = new();
    public SupportedLanguages currentLanguage = SupportedLanguages.EN;
    public AchievementProgressData achievementProgressData = new();
    
}
