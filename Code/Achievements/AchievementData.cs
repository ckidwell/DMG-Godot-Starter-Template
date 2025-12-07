using Godot;
using System;
using System.Collections.Generic;

public partial class AchievementData : Node
{
    public Dictionary<Achievements, bool> achievementsUnlocked = new()
    {
        {Achievements.WELCOME_FIRST_TIME, false},
        {Achievements.DIED_FIRST_TIME, false},
        {Achievements.KILL_1, false},
    };
}
