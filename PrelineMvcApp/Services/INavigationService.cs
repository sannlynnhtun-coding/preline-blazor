using PrelineMvcApp.Models.Navigation;

namespace PrelineMvcApp.Services;

public interface INavigationService
{
    IReadOnlyList<NavItem> Menu { get; }
    NavItem? Resolve(string path);
    IReadOnlyList<BreadcrumbItem> BuildBreadcrumbs(string path);
}
