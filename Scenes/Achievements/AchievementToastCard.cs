using Godot;

namespace DMGStarterTemplate;

public partial class AchievementToastCard : PanelContainer
{
    [Export] private Label nameLabel;
    [Export] private Label descriptionLabel;
    [Export] private PanelContainer card;
    [Export] private TextureButton closeButton;
    [Export] private TextureRect achievementIconTextureRect;

    // Seconds the toast stays fully visible before it starts to fade.
    [Export] private double DELAY_TO_FADE = 5;
    // Seconds the fade-out takes once it starts.
    [Export] private double FADE_SECONDS = 1.5;

    private Timer timer;
    private AnimationPlayer _animationPlayer;
    private GameEvents _gameEvents;
    private Tween _fadeTween;

    public override void _Ready()
    {
        _gameEvents = GetNode<GameEvents>("/root/GameEvents");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;

        closeButton.Pressed += OnClosePressed;
        
        timer = GetNode<Timer>("Timer");
        timer.OneShot = true;
        timer.Timeout += StartFade;
        timer.Start(DELAY_TO_FADE);
    }

    private void OnClosePressed()
    {
        timer.Stop();
        _fadeTween?.Kill();
        QueueFree();
    }

    private void StartFade()
    {
        MouseFilter = MouseFilterEnum.Ignore;
        closeButton.Disabled = true;
        
        _fadeTween = CreateTween();
        _fadeTween.TweenProperty(card, "modulate:a", 0f, FADE_SECONDS)
            .SetEase(Tween.EaseType.In);
        _fadeTween.Finished += QueueFree;
    }

    private void OnMouseEntered()
    {
        _gameEvents.EmitPlayAudioStream(GameConstants.UI_CLICK_BUTTON);
        _animationPlayer.Play("selected");
    }

    private void OnMouseExited()
    {
        _animationPlayer.Play("RESET");
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
