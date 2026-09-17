namespace DMGStarterTemplate;

public static class SupportedLanguagesExtensions
{
    // Godot locale codes (ISO 639-1). These must match the imported translation files
    // ("...Sheet1.ja.translation") and the column headers in the localization CSV.
    // Japanese is "ja", not "jp": the OS reports "ja_JP", and only "ja" matches it.
    public static string ToLocale(this SupportedLanguages language) => language switch
    {
        SupportedLanguages.EN => "en",
        SupportedLanguages.ES => "es",
        SupportedLanguages.FR => "fr",
        SupportedLanguages.DE => "de",
        SupportedLanguages.IT => "it",
        SupportedLanguages.JP => "ja",
        _ => "en",
    };

    // Translation key for the language's display name in the language selector.
    // Each key is a row in the localization CSV, so the list reads correctly in every locale.
    public static string DisplayNameKey(this SupportedLanguages language) => language switch
    {
        SupportedLanguages.EN => "English",
        SupportedLanguages.ES => "Spanish",
        SupportedLanguages.FR => "French",
        SupportedLanguages.DE => "German",
        SupportedLanguages.IT => "Italian",
        SupportedLanguages.JP => "Japanese",
        _ => language.ToString(),
    };
}
