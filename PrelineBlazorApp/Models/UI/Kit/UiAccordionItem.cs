using Microsoft.AspNetCore.Components;

namespace PrelineBlazorApp.Models.UI.Kit;

public sealed record UiAccordionItem(string Key, string Title, RenderFragment? Content = null, bool IsInitiallyOpen = false);
