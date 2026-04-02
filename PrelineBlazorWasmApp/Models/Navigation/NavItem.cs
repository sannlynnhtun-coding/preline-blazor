namespace PrelineBlazorWasmApp.Models.Navigation;

public sealed record NavItem(
    string Title,
    string Href,
    string Icon,
    string Section,
    string Description);
