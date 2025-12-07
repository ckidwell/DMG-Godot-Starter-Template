using Godot;
using System;


public partial class ToastManager : CanvasLayer
{
    [Export] private PackedScene AchievementToastCard;
    [Export] private VBoxContainer AchievementToastContainer;
    private GameEvents _gameEvents;
    
    public override void _Ready()
    {
        _gameEvents = GetNode<GameEvents>("/root/GameEvents");
        _gameEvents.AchievementEarned += OnAchievementEarned;
    }
    
    private void OnAchievementEarned(AchievementDescriptionVariant adv)
    {
        var _CardInstance  = AchievementToastCard.Instantiate() as AchievementToastCard;
        if (_CardInstance == null) return;
        
        _CardInstance.SetDescriptionText(adv.ad);
        
        AchievementToastContainer.AddChild(_CardInstance);
    }
}
