using System.Text.RegularExpressions;
using Microsoft.JSInterop;
using PrelineBlazorApp.Models.Theme;

namespace PrelineBlazorApp.Services;

public sealed class ThemeService(IJSRuntime jsRuntime) : IThemeService
{
    private static readonly Regex HexColorRegex = new("^#[0-9a-fA-F]{6}$", RegexOptions.Compiled);

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

        var normalized = palette.ToLowerInvariant();
        _cachedPreference!.DefaultPalette = normalized;
        await SafeInvokeAsync("dashboardTheme.setDefaultPalette", normalized);
    }

    public async Task SetDefaultCustomAccentAsync(string? hexColor)
    {
        await EnsureLoadedAsync();
        var normalized = NormalizeHexColor(hexColor);
        _cachedPreference!.DefaultCustomAccent = normalized;
        await SafeInvokeAsync("dashboardTheme.setDefaultCustomAccent", normalized);
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

    public async Task SetPageCustomAccentAsync(string route, string? hexColor)
    {
        await EnsureLoadedAsync();
        var normalizedRoute = NormalizeRoute(route);
        var normalized = NormalizeHexColor(hexColor);

        if (normalized is null)
        {
            _cachedPreference!.PageCustomAccentOverrides.Remove(normalizedRoute);
        }
        else
        {
            _cachedPreference!.PageCustomAccentOverrides[normalizedRoute] = normalized;
        }

        await SafeInvokeAsync("dashboardTheme.setPageCustomAccent", normalizedRoute, normalized);
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

        _cachedPreference.PagePaletteOverrides = _cachedPreference.PagePaletteOverrides is null
            ? new(StringComparer.OrdinalIgnoreCase)
            : new Dictionary<string, string>(_cachedPreference.PagePaletteOverrides, StringComparer.OrdinalIgnoreCase);

        _cachedPreference.PageCustomAccentOverrides = _cachedPreference.PageCustomAccentOverrides is null
            ? new(StringComparer.OrdinalIgnoreCase)
            : new Dictionary<string, string>(_cachedPreference.PageCustomAccentOverrides, StringComparer.OrdinalIgnoreCase);

        _cachedPreference.DefaultCustomAccent = NormalizeHexColor(_cachedPreference.DefaultCustomAccent);
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

    private static string? NormalizeHexColor(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }

        var value = input.Trim();
        if (!value.StartsWith('#'))
        {
            value = "#" + value;
        }

        if (value.Length == 4)
        {
            value = $"#{value[1]}{value[1]}{value[2]}{value[2]}{value[3]}{value[3]}";
        }

        if (!HexColorRegex.IsMatch(value))
        {
            return null;
        }

        return value.ToLowerInvariant();
    }
}
