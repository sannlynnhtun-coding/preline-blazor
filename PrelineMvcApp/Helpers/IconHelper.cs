using Microsoft.AspNetCore.Html;

namespace PrelineMvcApp.Helpers;

public static class IconHelper
{
    public static HtmlString GetIcon(string icon) => new HtmlString(icon switch
    {
        "chart-pie" => "<svg viewBox='0 0 24 24' class='size-4' fill='none' stroke='currentColor' stroke-width='1.8'><path d='M21 12a9 9 0 1 1-9-9'/><path d='M13 2v8h8'/></svg>",
        "line-chart" => "<svg viewBox='0 0 24 24' class='size-4' fill='none' stroke='currentColor' stroke-width='1.8'><path d='M3 3v18h18'/><path d='m19 9-5 5-4-4-4 4'/></svg>",
        "users" => "<svg viewBox='0 0 24 24' class='size-4' fill='none' stroke='currentColor' stroke-width='1.8'><path d='M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2'/><circle cx='9' cy='7' r='4'/><path d='M23 21v-2a4 4 0 0 0-3-3.87'/><path d='M16 3.13a4 4 0 0 1 0 7.75'/></svg>",
        "shopping-cart" => "<svg viewBox='0 0 24 24' class='size-4' fill='none' stroke='currentColor' stroke-width='1.8'><circle cx='8' cy='21' r='1'/><circle cx='19' cy='21' r='1'/><path d='M2.05 2h3l2.68 12.39a2 2 0 0 0 2 1.61h7.72a2 2 0 0 0 2-1.61L22 7H6'/></svg>",
        "receipt" => "<svg viewBox='0 0 24 24' class='size-4' fill='none' stroke='currentColor' stroke-width='1.8'><path d='M4 2h16v20l-3-2-3 2-3-2-3 2-3-2-3 2z'/><path d='M8 7h8M8 11h8M8 15h5'/></svg>",
        "briefcase" => "<svg viewBox='0 0 24 24' class='size-4' fill='none' stroke='currentColor' stroke-width='1.8'><path d='M16 20V4a2 2 0 0 0-2-2h-4a2 2 0 0 0-2 2v16'/><rect x='2' y='7' width='20' height='13' rx='2'/></svg>",
        "check-square" => "<svg viewBox='0 0 24 24' class='size-4' fill='none' stroke='currentColor' stroke-width='1.8'><rect x='3' y='3' width='18' height='18' rx='2'/><path d='m9 12 2 2 4-4'/></svg>",
        "activity" => "<svg viewBox='0 0 24 24' class='size-4' fill='none' stroke='currentColor' stroke-width='1.8'><path d='M22 12h-4l-3 9L9 3 6 12H2'/></svg>",
        "panel-top" => "<svg viewBox='0 0 24 24' class='size-4' fill='none' stroke='currentColor' stroke-width='1.8'><rect x='3' y='4' width='18' height='16' rx='2'/><path d='M3 9h18'/></svg>",
        "settings" => "<svg viewBox='0 0 24 24' class='size-4' fill='none' stroke='currentColor' stroke-width='1.8'><path d='M12 1v2M12 21v2M4.2 4.2l1.4 1.4M18.4 18.4l1.4 1.4M1 12h2M21 12h2M4.2 19.8l1.4-1.4M18.4 5.6l1.4-1.4'/><circle cx='12' cy='12' r='4'/></svg>",
        _ => "<svg viewBox='0 0 24 24' class='size-4' fill='none' stroke='currentColor' stroke-width='1.8'><circle cx='12' cy='12' r='9'/></svg>"
    });
}
