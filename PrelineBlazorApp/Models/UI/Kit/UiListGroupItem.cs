namespace PrelineBlazorApp.Models.UI.Kit;

public sealed record UiListGroupItem(string Title, string? Subtitle = null, UiTone Tone = UiTone.Neutral, string? Href = null);
