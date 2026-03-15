using PrelineBlazorApp.Models.Theme;

namespace PrelineBlazorApp.Services;

public interface IThemeService
{
    Task<ThemePreference> GetAsync();
    Task SetModeAsync(ThemeMode mode);
    Task SetDefaultPaletteAsync(string palette);
    Task SetPagePaletteAsync(string route, string? palette);
    Task ApplyAsync(string route);
}
