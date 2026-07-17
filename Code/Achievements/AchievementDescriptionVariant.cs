using Godot;
using System;

namespace DMGStarterTemplate;

public partial class AchievementDescriptionVariant : Node
{
    public AchievementDescription ad;

    public AchievementDescriptionVariant(AchievementDescription achievementDescription)
    {
        ad = achievementDescription;
    }
}
