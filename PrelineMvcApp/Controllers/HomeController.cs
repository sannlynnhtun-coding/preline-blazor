using Microsoft.AspNetCore.Mvc;
using PrelineMvcApp.Services;

namespace PrelineMvcApp.Controllers;

public class HomeController : Controller
{
    private readonly IDashboardDataService _dashboardData;

    public HomeController(IDashboardDataService dashboardData)
    {
        _dashboardData = dashboardData;
    }

    public async Task<IActionResult> Index()
    {
        var kpis = await _dashboardData.GetKpisAsync();
        var orders = await _dashboardData.GetOrdersAsync();
        ViewBag.Kpis = kpis;
        ViewBag.Orders = orders;
        return View();
    }

    public async Task<IActionResult> Analytics()
    {
        ViewBag.Kpis = await _dashboardData.GetKpisAsync();
        return View();
    }

    public async Task<IActionResult> Customers()
    {
        var customers = await _dashboardData.GetCustomersAsync();
        return View(customers);
    }

    public async Task<IActionResult> Orders()
    {
        var orders = await _dashboardData.GetOrdersAsync();
        return View(orders);
    }

    public async Task<IActionResult> Billing()
    {
        var invoices = await _dashboardData.GetInvoicesAsync();
        return View(invoices);
    }

    public async Task<IActionResult> Team()
    {
        var members = await _dashboardData.GetTeamMembersAsync();
        return View(members);
    }

    public async Task<IActionResult> Tasks()
    {
        var tasks = await _dashboardData.GetTasksAsync();
        return View(tasks);
    }

    public async Task<IActionResult> Activity()
    {
        var activity = await _dashboardData.GetActivityAsync();
        return View(activity);
    }

    public IActionResult Settings() => View();

    public IActionResult Components() => View();

}
