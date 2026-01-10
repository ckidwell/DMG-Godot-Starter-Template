using Godot;
using System;

public partial class SaveGameData : Node
{
    public float musicVolume = .5f;
    public float soundVolume = .5f;
    public int currency = 0;
    public AchievementData achievementData = new();
    public SupportedLanguages currentLanguage = SupportedLanguages.EN;
    public AchievementProgressData achievementProgressData = new();
    
}
