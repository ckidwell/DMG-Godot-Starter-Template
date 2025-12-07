using Godot;
using System;

public class AchievementDescription
{
    public Achievements achievement;
    public string Title;
    public string Description;
    public string earnedFor;
    
    public static AchievementDescription GetDescriptionForAchievement(Achievements achievement)
    {
        switch (achievement)
        {
            case Achievements.NONE:
                return new AchievementDescription
                {
                    Title = "You've done nothing!",
                    Description = "You literally got an achievement for nothing.",
                    earnedFor = "This achievement is given away for free."
                };
            case Achievements.WELCOME_FIRST_TIME:
                return new AchievementDescription
                {
                    Title = "Welcome, and thanks!",
                    Description = "Thanks for playing, welcome to TEMPLATE GAME!",
                    earnedFor = "This earned for playing TEMPLATE GAME the first time."
                };
              
            case Achievements.DIED_FIRST_TIME:
                return new AchievementDescription
                {
                    Title = "Death is only the beginning.",
                    Description = "Congratulations, you died!",
                    earnedFor = "This earned for dying the first time."
                };

            case Achievements.KILL_1:
                return new AchievementDescription
                {
                    Title = "Are you one, Herbert?",
                    Description = "Well you managed to kill at least one monster!",
                    earnedFor = "This earned for killing your first monster."
                };
            
            default:
                throw new ArgumentOutOfRangeException(nameof(achievement), achievement, null);
        }
    }
}
