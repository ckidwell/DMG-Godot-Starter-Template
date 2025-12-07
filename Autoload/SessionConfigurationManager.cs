using Godot;


public partial class SessionConfigurationManager : Node
{
    private GameEvents _gameEvents;
    
    public static bool UNLOCK_ALL_DEBUG;
    private bool unlock_All_Debug;
    

    private bool shift_pressed;
    private bool f3_Pressed;

    private Timer keypressTimer;
    private bool keyDelayExpired = true;

    public override void _Ready()
    {
        _gameEvents = GetNode<GameEvents>("/root/GameEvents");

        keypressTimer = new Timer();
        AddChild(keypressTimer);
        keypressTimer.WaitTime =1.75d;
        keypressTimer.OneShot = true;
        keypressTimer.Timeout += OnTimerTimeout;
        keypressTimer.Start();
        
        UNLOCK_ALL_DEBUG = false;

    }

    private void OnTimerTimeout()
    {
        keyDelayExpired = true;
    }

    public override void _Process(double delta)
    {
        var shift = Input.IsActionPressed("SHIFT_BUTTON");
        var f3 = Input.IsActionPressed("F3_BUTTON");
        
        if (shift && f3 && keyDelayExpired)
        {
            keyDelayExpired = false;
            keypressTimer.Stop();
            keypressTimer.Start();
            
            UNLOCK_ALL_DEBUG = !UNLOCK_ALL_DEBUG;
            GD.Print($"Debug mode is: {UNLOCK_ALL_DEBUG}");
        }
    }

    
    public static bool IsDebugUnlocked()
    {
        return UNLOCK_ALL_DEBUG;
    }

    private void LoadMainScene()
    {
        GetTree().ChangeSceneToFile("res://scenes/Main/main.tscn");
    }
    public static void SetBusVolumePercent(string busName, float percent)
    {
        // all this is needed because DB's are not linear apparently
        // so we have to convert DB's to a percentage to use a volume slider
        var bus_index = AudioServer.GetBusIndex(busName);
        var volume_db = Mathf.LinearToDb(percent);
        AudioServer.SetBusVolumeDb(bus_index, volume_db);
    }

}
