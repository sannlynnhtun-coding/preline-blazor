using Microsoft.JSInterop;
using PrelineBlazorApp.Models.Theme;

namespace PrelineBlazorApp.Services;

public sealed class ThemeService(IJSRuntime jsRuntime) : IThemeService
{
    private ThemePreference? _cachedPreference;

    public async Task<ThemePreference> GetAsync()
    {
        await EnsureLoadedAsync();
        return _cachedPreference!.Clone();
    }

    public async Task SetModeAsync(ThemeMode mode)
    {
        await EnsureLoadedAsync();
        _cachedPreference!.Mode = mode;
        await SafeInvokeAsync("dashboardTheme.setMode", mode.ToString().ToLowerInvariant());
    }

    public async Task SetDefaultPaletteAsync(string palette)
    {
        await EnsureLoadedAsync();
        if (!ThemePalettes.IsKnown(palette))
        {
            palette = ThemePreference.CreateDefault().DefaultPalette;
        }

        _cachedPreference!.DefaultPalette = palette;
        await SafeInvokeAsync("dashboardTheme.setDefaultPalette", palette);
    }

    public async Task SetPagePaletteAsync(string route, string? palette)
    {
        await EnsureLoadedAsync();
        var normalizedRoute = NormalizeRoute(route);
        string? valueToApply = null;

        if (string.IsNullOrWhiteSpace(palette))
        {
            _cachedPreference!.PagePaletteOverrides.Remove(normalizedRoute);
        }
        else
        {
            var validatedPalette = ThemePalettes.IsKnown(palette)
                ? palette.ToLowerInvariant()
                : ThemePreference.CreateDefault().DefaultPalette;

            _cachedPreference!.PagePaletteOverrides[normalizedRoute] = validatedPalette;
            valueToApply = validatedPalette;
        }

        await SafeInvokeAsync("dashboardTheme.setPagePalette", normalizedRoute, valueToApply);
    }

    public async Task ApplyAsync(string route)
    {
        await EnsureLoadedAsync();
        await SafeInvokeAsync("dashboardTheme.apply", NormalizeRoute(route));
    }

    private async Task EnsureLoadedAsync()
    {
        if (_cachedPreference is not null)
        {
            return;
        }

        try
        {
            _cachedPreference = await jsRuntime.InvokeAsync<ThemePreference>("dashboardTheme.getPreference");
        }
        catch
        {
            _cachedPreference = ThemePreference.CreateDefault();
            return;
        }

        _cachedPreference ??= ThemePreference.CreateDefault();
    }

    private async Task SafeInvokeAsync(string identifier, params object?[]? args)
    {
        try
        {
            if (args is null)
            {
                await jsRuntime.InvokeVoidAsync(identifier);
                return;
            }

            await jsRuntime.InvokeVoidAsync(identifier, args);
        }
        catch
        {
            // Theme persistence falls back to in-memory values when JS interop is unavailable.
        }
    }

    private static string NormalizeRoute(string route)
    {
        if (string.IsNullOrWhiteSpace(route))
        {
            return "/";
        }

        var normalized = route.Trim();
        if (!normalized.StartsWith('/'))
        {
            normalized = "/" + normalized;
        }

        return normalized.TrimEnd('/') is { Length: > 0 } value ? value : "/";
    }
}

