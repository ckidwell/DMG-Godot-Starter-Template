using Godot;
using System;

namespace DMGStarterTemplate;

public partial class SaveGameDataVariant : Node
{
    public SaveGameDataVariant(SaveGameData saveGameData)
    {
        SaveGameData = saveGameData;
    }
    public SaveGameData SaveGameData;
}
