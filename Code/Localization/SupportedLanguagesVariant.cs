using Godot;
using System;

public partial class SupportedLanguagesVariant : Node
{
    public SupportedLanguages sl;

    public SupportedLanguagesVariant(SupportedLanguages supportedLanguages)
    {
        sl = supportedLanguages;
    }
}
