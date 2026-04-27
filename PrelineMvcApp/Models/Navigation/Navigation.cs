namespace PrelineMvcApp.Models.Navigation;

public record NavItem(string Title, string Href, string Icon, string Section, string? Description = null);

public record BreadcrumbItem(string Title, string? Href = null);
