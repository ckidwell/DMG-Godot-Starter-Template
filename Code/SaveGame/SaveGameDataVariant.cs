using Godot;
using System;

public partial class SaveGameDataVariant : Node
{
    public SaveGameDataVariant(SaveGameData saveGameData)
    {
        SaveGameData = saveGameData;
    }
    public SaveGameData SaveGameData;
}
