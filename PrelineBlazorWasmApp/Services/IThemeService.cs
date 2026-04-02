using PrelineBlazorWasmApp.Models.Theme;

namespace PrelineBlazorWasmApp.Services;

public interface IThemeService
{
    Task<ThemePreference> GetAsync();
    Task SetModeAsync(ThemeMode mode);
    Task SetDefaultPaletteAsync(string palette);
    Task SetDefaultCustomAccentAsync(string? hexColor);
    Task SetPagePaletteAsync(string route, string? palette);
    Task SetPageCustomAccentAsync(string route, string? hexColor);
    Task ApplyAsync(string route);
}
