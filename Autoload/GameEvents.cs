using Godot;
using System;

public partial class GameEvents : Node
{
    [Signal]
    public delegate void SaveGameDataUpdatedEventHandler(SaveGameDataVariant data);
    
    [Signal]
    public delegate void PlayAudioStreamEventHandler(string soundEventName);
    
    [Signal]
    public delegate void CurrencyPickedUpEventHandler(int value);
    
    [Signal]
    public delegate void CurrencyUpdatedEventHandler(int value);
    
    [Signal]
    public delegate void MainVolumeEventHandler(float amount);
    
    [Signal]
    public delegate void MusicVolumeEventHandler(float amount);

    [Signal]
    public delegate void SoundVolumeEventHandler(float amount);
    
    [Signal]  
    public delegate void DiedEventHandler(Vector2 pos, bool isPlayer, double experienceDropPercentage);
    
    [Signal]
    public delegate void RePoolMeEventHandler(ulong mySpawner, Node2D item);
    
    [Signal]
    public delegate void ScreenShakeEventHandler(float duration, float strength, float size, float rampTime, float rampStrength);
    
    [Signal]
    public delegate void SupportedLanguageUpdatedEventHandler(SupportedLanguagesVariant language);
    
    [Signal]
    public delegate void AchievementEarnedEventHandler(AchievementDescriptionVariant adv);
    
    public void EmitSaveGameDataUpdated(SaveGameDataVariant data)
    {
        EmitSignal(SignalName.SaveGameDataUpdated, data);
    }
    public void EmitPlayAudioStream(string soundEventName)
    {
        EmitSignal(SignalName.PlayAudioStream, soundEventName);
    }
    public void EmitCurrencyPickedUp(int value)
    {
        EmitSignal(SignalName.CurrencyPickedUp, value);
    }
    public void EmitCurrencyUpdated(int value)
    {
        EmitSignal(SignalName.CurrencyUpdated, value);
    }

    public void EmitMainVolume(float amount)
    {
        EmitSignal(SignalName.MainVolume, amount);
    }
    public void EmitMusicVolume(float amount)
    {
        EmitSignal(SignalName.MusicVolume, amount);
    }

    public void EmitSoundVolume(float amount)
    {
        EmitSignal(SignalName.SoundVolume, amount);
    }
    public void EmitDied(Vector2 pos, bool isPlayer, double experienceDropPercentage)
    {
        EmitSignal(SignalName.Died,pos, isPlayer, experienceDropPercentage);
    }
    public void EmitRePoolMe(ulong mySpawner, Node2D item)
    {
        EmitSignal(SignalName.RePoolMe, mySpawner, item);
    }
    public void EmitScreenShake(float duration, float strength, float strengthDecayRate, float rampTime, float rampStrength)
    {
        EmitSignal(SignalName.ScreenShake, duration, strength, strengthDecayRate, rampTime, rampStrength);
    }
    public void EmitSupportedLanguageUpdated(SupportedLanguagesVariant slv)
    {
        EmitSignal(SignalName.SupportedLanguageUpdated, slv);
    }
    public void EmitAchievementEarned(AchievementDescriptionVariant adv)
    {
        EmitSignal(SignalName.AchievementEarned, adv);
    }

}
