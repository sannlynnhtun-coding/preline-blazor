using PrelineBlazorApp.Models.Navigation;

namespace PrelineBlazorApp.Services;

public interface INavigationService
{
    IReadOnlyList<NavItem> Menu { get; }
    NavItem? Resolve(string path);
    IReadOnlyList<BreadcrumbItem> BuildBreadcrumbs(string path);
}
