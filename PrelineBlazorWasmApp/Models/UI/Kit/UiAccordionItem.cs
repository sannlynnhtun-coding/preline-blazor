using Microsoft.AspNetCore.Components;

namespace PrelineBlazorWasmApp.Models.UI.Kit;

public sealed record UiAccordionItem(string Key, string Title, RenderFragment? Content = null, bool IsInitiallyOpen = false);
