using System;
using System.Collections.Generic;
using System.Linq;

namespace DMGStarterTemplate;

// Plain serializable DTO — see SaveGameData for why this is not a Godot Node.
public class AchievementData
{
    public Dictionary<Achievements, bool> achievementsUnlocked =
        Enum.GetValues<Achievements>()
            .Where(a => a != Achievements.NONE)
            .ToDictionary(a => a, _ => false);
}
