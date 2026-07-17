using Godot;

namespace DMGStarterTemplate;

// RefCounted (not Node) so these short-lived signal-payload wrappers free themselves
// instead of leaking — a Node that is never added to the tree is never freed.
public partial class SaveGameDataVariant : RefCounted
{
    public SaveGameDataVariant(SaveGameData saveGameData)
    {
        SaveGameData = saveGameData;
    }
    public SaveGameData SaveGameData;
}
