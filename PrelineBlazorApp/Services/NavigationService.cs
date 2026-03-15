using PrelineBlazorApp.Models.Navigation;

namespace PrelineBlazorApp.Services;

public sealed class NavigationService : INavigationService
{
    public IReadOnlyList<NavItem> Menu { get; } =
    [
        new("Dashboard", "/dashboard", "chart-pie", "Overview", "Business snapshot and KPIs"),
        new("Analytics", "/analytics", "line-chart", "Overview", "Funnel, growth and channel trends"),
        new("Customers", "/customers", "users", "Management", "Segments and retention health"),
        new("Orders", "/orders", "shopping-cart", "Management", "Order throughput and fulfilment"),
        new("Billing", "/billing", "receipt", "Management", "Invoices, dues and payment state"),
        new("Team", "/team", "briefcase", "Workspace", "Team presence and ownership"),
        new("Tasks", "/tasks", "check-square", "Workspace", "Operational work queue"),
        new("Activity", "/activity", "activity", "Workspace", "Audit-style activity stream"),
        new("Components", "/components", "panel-top", "System", "Reusable UI building blocks"),
        new("Settings", "/settings", "settings", "System", "Theme and dashboard defaults")
    ];

    public NavItem? Resolve(string path)
    {
        var normalized = Normalize(path);
        return Menu.FirstOrDefault(x => string.Equals(x.Href, normalized, StringComparison.OrdinalIgnoreCase))
            ?? Menu.FirstOrDefault(x => normalized == "/" && x.Href == "/dashboard");
    }

    public IReadOnlyList<BreadcrumbItem> BuildBreadcrumbs(string path)
    {
        var current = Resolve(path);
        if (current is null)
        {
            return [new BreadcrumbItem("Workspace", "/dashboard"), new BreadcrumbItem("Unknown")];
        }

        return
        [
            new BreadcrumbItem("Workspace", "/dashboard"),
            new BreadcrumbItem(current.Section),
            new BreadcrumbItem(current.Title)
        ];
    }

    private static string Normalize(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return "/";
        }

        var normalized = path.StartsWith('/') ? path : "/" + path;
        normalized = normalized.Split('?', '#')[0];
        return normalized.TrimEnd('/') is { Length: > 0 } value ? value : "/";
    }
}
