using Godot;
using System;

public partial class GameConstants : Node
{
    public static Vector2 DESPAWN_LOCATION { get; private set; } = new Vector2(+10000, +10000);
    
    public const string MAIN_BUS = "Master";
    public const string MUSIC_BUS = "Music";
    public const string EFFECTS_BUS = "SFX";
    
    // UI Events
    public const string UI_CLICK_BUTTON = "UI_CLICK_BUTTON";

    public static Color DAMAGE_COLOR_LIGHT_GREEN => new Color(0.813f, 0.997f, 0.919f);
    
    // projectile fire events
    public const string P_BULLET = "bullet";
    
    //SOUNDS

    public const string S_EXPLOSION = "EXPLOSION";
    public const string S_HIT = "HIT";
    public const string S_XP_GEM_COLLECTED = "XP_GEM_COLLCETED";
    public const string S_BULLET_FIRED = "BULLET_FIRED";
    public const string S_COIN_COLLECTED = "COIN_COLLECTED";
    public const string S_HEALTH_COLLECTED = "HEALTH_COLLECTED";

}
