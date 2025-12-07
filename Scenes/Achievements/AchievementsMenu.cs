using Godot;

public partial class AchievementsMenu : CanvasLayer
{
    private Button _backButton;
    private GridContainer _GridContainer;

    [Export] private PackedScene AchievementDisplayCard;
    
    private GameEvents _gameEvents;
    
    public override void _Ready()
    {
        _gameEvents = GetNode<GameEvents>("/root/GameEvents");
        _backButton = GetNode<Button>("%BackButton");
        _backButton.Pressed += OnBackButtonPressed;
         
        _GridContainer = GetNode<GridContainer>("%AchievementGridContainer");

        var achievements = ProgressionManager.GetSaveGameData().achievementData.achievementsUnlocked;
         
        foreach (var achievement in achievements)
        {
            var achievementCard = AchievementDisplayCard.Instantiate() as AchievementCard;
            if (achievementCard == null) continue;
            
            achievementCard.SetAchievement(achievement.Key, achievement.Value);
            achievementCard.SetDescriptionText(AchievementDescription.GetDescriptionForAchievement(achievement.Key));
            achievementCard.SetUnlockedTexture();
            
            _GridContainer.AddChild(achievementCard);
        }
    }
    
    
    private void OnBackButtonPressed()
    {
        // _gameEvents.EmitShowTransitionScreen();
        // var waitForIt = Callable.From(QueueFree);
        // Task.Delay(850).ContinueWith( (t) => waitForIt.CallDeferred());
    }
}
