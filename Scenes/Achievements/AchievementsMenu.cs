using Godot;

public partial class AchievementsMenu : CanvasLayer
{
    private Button _backButton;
    private GridContainer _GridContainer;

    [Export] private PackedScene AchievementDisplayCard;
    
    private MenuSystemManager _menuSystemManager;
    private GameEvents _gameEvents;
    
    public override void _Ready()
    {
        _gameEvents = GetNode<GameEvents>("/root/GameEvents");
        _menuSystemManager = GetNode<MenuSystemManager>("/root/MenuSystemManager");
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
        _gameEvents.EmitPlayAudioStream(GameConstants.UI_CLICK_BUTTON);
        _menuSystemManager.SetCurrentMenu(MenuType.MAIN);
    }
}
