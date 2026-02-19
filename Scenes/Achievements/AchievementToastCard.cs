using Godot;
using System;
using System.Threading.Tasks;

namespace MyGame;

public partial class AchievementToastCard : PanelContainer
{
			[Export] private Label nameLabel;
            [Export] private Label descriptionLabel;
            [Export] private PanelContainer card;
            [Export] private TextureButton closeButton;
            private Timer timer;
            [Export] private double DELAY_TO_FADE = 5;
   
            [Export] private TextureRect achievementIconTextureRect;
            private Achievements _achievement;
        	private AnimationPlayer _animationPlayer;
    
        	private bool _achievementLocked = true;

	        private GameEvents _gameEvents;

	        private bool dispose = false;
        
        	public override void _Ready()
        	{
		        _gameEvents = GetNode<GameEvents>("/root/GameEvents");
        		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        		
        		MouseEntered += OnMouseEntered;
        		MouseExited += OnMouseExited;
		        
		        timer = GetNode<Timer>("Timer");
		        timer.WaitTime = DELAY_TO_FADE;
		        timer.Start(DELAY_TO_FADE);
		        timer.Timeout += OnTimerTimeout;

		        closeButton.Pressed += OnClosePressed;
	        }

	        private void OnClosePressed()
	        {
		        timer.Stop();
		        QueueFree();
	        }

	        public override void _Process(double delta)
	        {
		        if (dispose) return;

		        if (!(timer.TimeLeft <= 0)) return;
		        
		        var currentColor = card.Modulate;
		        var newColor = new Color(1, 1, 1, Mathf.Lerp(currentColor.A, 0, .01f));
		        card.Modulate = newColor;

		        if (currentColor.A >= .02) return;

		        dispose = true;
		        
		        var waitForIt = Callable.From(QueueFree);
		        Task.Delay(150).ContinueWith( (t) => waitForIt.CallDeferred());
	        }
	        private void OnTimerTimeout()
	        {
		        timer.Stop();
	        }

	        private void OnMouseExited()
        	{
        		_animationPlayer.Play("RESET");
        	}
        
        	private void OnMouseEntered()
        	{
		        _gameEvents.EmitPlayAudioStream(GameConstants.S_HIT);
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
