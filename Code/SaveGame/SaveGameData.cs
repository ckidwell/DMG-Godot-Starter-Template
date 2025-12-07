using Godot;
using System;

public partial class SaveGameData : Node
{
    public float musicVolume = 100;
    public float soundVolume = 100;
    public int currency = 0;
    public AchievementData achievementData = new();
    public SupportedLanguages currentLanguage = SupportedLanguages.EN;
    public AchievementProgressData achievementProgressData = new();
    
}
