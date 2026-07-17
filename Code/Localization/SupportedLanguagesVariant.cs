using Godot;
using System;

namespace DMGStarterTemplate;

public partial class SupportedLanguagesVariant : Node
{
    public SupportedLanguages sl;

    public SupportedLanguagesVariant(SupportedLanguages supportedLanguages)
    {
        sl = supportedLanguages;
    }
}
