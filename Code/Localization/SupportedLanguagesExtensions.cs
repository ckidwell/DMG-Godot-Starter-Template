namespace DMGStarterTemplate;

public static class SupportedLanguagesExtensions
{
    // Locale codes match the imported translation files (e.g. "...Sheet1.en.translation").
    public static string ToLocale(this SupportedLanguages language) => language switch
    {
        SupportedLanguages.EN => "en",
        SupportedLanguages.ES => "es",
        SupportedLanguages.FR => "fr",
        SupportedLanguages.DE => "de",
        SupportedLanguages.IT => "it",
        SupportedLanguages.JP => "jp",
        _ => "en",
    };
}
