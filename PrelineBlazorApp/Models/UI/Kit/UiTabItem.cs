using Microsoft.AspNetCore.Components;

namespace PrelineBlazorApp.Models.UI.Kit;

public sealed record UiTabItem(string Key, string Label, RenderFragment? Content = null, bool Disabled = false);
