using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PrelineBlazorWasmApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<PrelineBlazorWasmApp.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<IThemeService, ThemeService>();
builder.Services.AddScoped<IDashboardDataService, MockDashboardDataService>();
builder.Services.AddSingleton<INavigationService, NavigationService>();

await builder.Build().RunAsync();
