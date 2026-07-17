using System.Collections.Generic;

namespace DMGStarterTemplate;

// Plain serializable DTO — see SaveGameData for why this is not a Godot Node.
public class AchievementData
{
    public Dictionary<Achievements, bool> achievementsUnlocked = new()
    {
        {Achievements.WELCOME_FIRST_TIME, false},
        {Achievements.DIED_FIRST_TIME, false},
        {Achievements.KILL_1, false},
    };
}
