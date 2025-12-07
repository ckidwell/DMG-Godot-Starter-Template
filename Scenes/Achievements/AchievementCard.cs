using Godot;

public partial class AchievementCard : PanelContainer
{
		[Export] private Label nameLabel;
        [Export] private Label descriptionLabel;
        [Export] private Texture2D unlockedIcon;
        [Export] private TextureRect unlockedStatusTextureRect;
        [Export] private TextureRect achievementIconTextureRect;
        private Achievements _achievement;
    	private AnimationPlayer _animationPlayer;

    	private bool _achievementLocked = true;
	    
	    private GameEvents _gameEvents;

	    
    	public override void _Ready()
    	{
		    _gameEvents = GetNode<GameEvents>("/root/GameEvents");
		    _gameEvents.SaveGameDataUpdated += OnSaveGameDataUpdated;
    		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    		
    		MouseEntered += OnMouseEntered;
    		MouseExited += OnMouseExited;
    		UpdateLockedLabel();
    	}
    
    	private void OnSaveGameDataUpdated(SaveGameDataVariant data)
    	{
    		if (data.SaveGameData.achievementData.achievementsUnlocked.TryGetValue(_achievement, out var ach))
    		{
    			_achievementLocked = ach;
    		}
    		UpdateLockedLabel();
    	}
	    
    	private void UpdateLockedLabel()
	    {
		    if (_achievementLocked) return;

		    unlockedStatusTextureRect.Texture = unlockedIcon;

	    }
    	private void OnMouseExited()
    	{
    		_animationPlayer.Play("RESET");
    	}
    
    	private void OnMouseEntered()
    	{
		    _gameEvents.EmitPlayAudioStream(GameConstants.S_HIT);
    	}
   
    	public void SetUnlockedTexture()
	    {
		    UpdateLockedLabel();
    	}

	    public void SetAchievement(Achievements ach, bool unlocked)
	    {
		    _achievement = ach;
		    _achievementLocked = !unlocked;
	    }
	    public void SetAchievementIcon(Texture2D texture)
	    {
		    achievementIconTextureRect.Texture = texture;
	    }
    	public void SetDescriptionText(AchievementDescription description)
	    {
		    nameLabel.Text = description.Title;
    		descriptionLabel.Text = description.Description;
    	}
}
