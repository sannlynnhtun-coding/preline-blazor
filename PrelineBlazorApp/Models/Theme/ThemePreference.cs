namespace PrelineBlazorApp.Models.Theme;

public sealed class ThemePreference
{
    public ThemeMode Mode { get; set; } = ThemeMode.System;
    public string DefaultPalette { get; set; } = "slate";
    public Dictionary<string, string> PagePaletteOverrides { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    public static ThemePreference CreateDefault() => new();

    public ThemePreference Clone()
    {
        return new ThemePreference
        {
            Mode = Mode,
            DefaultPalette = DefaultPalette,
            PagePaletteOverrides = PagePaletteOverrides.ToDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase)
        };
    }
}
