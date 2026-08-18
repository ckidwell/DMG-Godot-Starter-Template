using Godot;

namespace DMGStarterTemplate;

// Helpers for converting between a 0..1 slider percentage and an audio bus's dB volume.
// dB is not linear, so a volume slider (linear 0..1) must be converted to/from dB.
public static class AudioBus
{
    public static void SetVolumePercent(string busName, float percent)
    {
        var busIndex = AudioServer.GetBusIndex(busName);
        AudioServer.SetBusVolumeDb(busIndex, Mathf.LinearToDb(percent));
    }

    public static float GetVolumePercent(string busName)
    {
        var busIndex = AudioServer.GetBusIndex(busName);
        return Mathf.DbToLinear(AudioServer.GetBusVolumeDb(busIndex));
    }
}
