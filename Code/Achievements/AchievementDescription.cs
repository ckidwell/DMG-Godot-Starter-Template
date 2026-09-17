using System;

namespace DMGStarterTemplate;

// Achievement text is stored as translation KEYS, not English strings. The rows live in
// Localization/game_template_localization - Sheet1.csv. Labels translate the keys themselves
// (Control.auto_translate is on by default), so assigning a key to Label.Text is enough and the
// text also refreshes live when the player changes language.
//
// To add an achievement: add the enum value, add a case here, add the three rows to the CSV.
public class AchievementDescription
{
    public Achievements achievement;
    public string TitleKey;
    public string DescriptionKey;
    public string EarnedForKey;

    public static AchievementDescription GetDescriptionForAchievement(Achievements achievement)
    {
        switch (achievement)
        {
            case Achievements.NONE:
                return ForKeys(achievement, "ACH_NONE");
            case Achievements.WELCOME_FIRST_TIME:
                return ForKeys(achievement, "ACH_WELCOME_FIRST_TIME");
            case Achievements.DIED_FIRST_TIME:
                return ForKeys(achievement, "ACH_DIED_FIRST_TIME");
            case Achievements.KILL_1:
                return ForKeys(achievement, "ACH_KILL_1");
            default:
                throw new ArgumentOutOfRangeException(nameof(achievement), achievement, null);
        }
    }

    private static AchievementDescription ForKeys(Achievements achievement, string prefix) => new()
    {
        achievement = achievement,
        TitleKey = prefix + "_TITLE_",
        DescriptionKey = prefix + "_DESC_",
        EarnedForKey = prefix + "_EARNED_",
    };
}
