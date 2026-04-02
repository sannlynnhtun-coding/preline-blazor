namespace PrelineBlazorWasmApp.Models.Theme;

public sealed class ThemePreference
{
    public ThemeMode Mode { get; set; } = ThemeMode.System;
    public string DefaultPalette { get; set; } = "slate";
    public string? DefaultCustomAccent { get; set; }
    public Dictionary<string, string> PagePaletteOverrides { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public Dictionary<string, string> PageCustomAccentOverrides { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    public static ThemePreference CreateDefault() => new();

    public ThemePreference Clone()
    {
        return new ThemePreference
        {
            Mode = Mode,
            DefaultPalette = DefaultPalette,
            DefaultCustomAccent = DefaultCustomAccent,
            PagePaletteOverrides = PagePaletteOverrides.ToDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase),
            PageCustomAccentOverrides = PageCustomAccentOverrides.ToDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase)
        };
    }
}
