using PrelineMvcApp.Models.Navigation;

namespace PrelineMvcApp.Services;

public sealed class NavigationService : INavigationService
{
    public IReadOnlyList<NavItem> Menu { get; } =
    [
        new("Dashboard", "/", "chart-pie", "Overview", "Business snapshot and KPIs"),
        new("Analytics", "/Home/Analytics", "line-chart", "Overview", "Funnel, growth and channel trends"),
        new("Customers", "/Home/Customers", "users", "Management", "Segments and retention health"),
        new("Orders", "/Home/Orders", "shopping-cart", "Management", "Order throughput and fulfilment"),
        new("Billing", "/Home/Billing", "receipt", "Management", "Invoices, dues and payment state"),
        new("Team", "/Home/Team", "briefcase", "Workspace", "Team presence and ownership"),
        new("Tasks", "/Home/Tasks", "check-square", "Workspace", "Operational work queue"),
        new("Activity", "/Home/Activity", "activity", "Workspace", "Audit-style activity stream"),
        new("Components", "/Home/Components", "panel-top", "System", "Reusable UI building blocks"),
        new("Settings", "/Home/Settings", "settings", "System", "Theme and dashboard defaults")
    ];

    public NavItem? Resolve(string path)
    {
        var normalized = Normalize(path);
        return Menu.FirstOrDefault(x => string.Equals(x.Href, normalized, StringComparison.OrdinalIgnoreCase))
            ?? Menu.FirstOrDefault(x => normalized == "/" && (x.Href == "/" || x.Href == "/Home/Index"));
    }

    public IReadOnlyList<BreadcrumbItem> BuildBreadcrumbs(string path)
    {
        var current = Resolve(path);
        if (current is null)
        {
            return [new BreadcrumbItem("Workspace", "/"), new BreadcrumbItem("Unknown")];
        }

        return
        [
            new BreadcrumbItem("Workspace", "/"),
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
