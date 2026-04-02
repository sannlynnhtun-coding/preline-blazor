namespace PrelineBlazorWasmApp.Models.Theme;

public sealed record ThemePalette(string Key, string Name, string AccentHex, string SoftHex);

public static class ThemePalettes
{
    public static IReadOnlyList<ThemePalette> All { get; } =
    [
        new("slate", "Slate", "#334155", "#e2e8f0"),
        new("sky", "Sky", "#0369a1", "#e0f2fe"),
        new("emerald", "Emerald", "#047857", "#d1fae5"),
        new("amber", "Amber", "#b45309", "#fef3c7"),
        new("rose", "Rose", "#be123c", "#ffe4e6")
    ];

    public static bool IsKnown(string? key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return false;
        }

        return All.Any(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
    }
}
