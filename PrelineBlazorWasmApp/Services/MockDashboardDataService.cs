using PrelineBlazorWasmApp.Models.Dashboard;

namespace PrelineBlazorWasmApp.Services;

public sealed class MockDashboardDataService : IDashboardDataService
{
    private static readonly IReadOnlyList<KpiMetric> Kpis =
    [
        new("MRR", "$284,200", "+12.7%", true, "Compared with last month"),
        new("Active Users", "18,492", "+4.2%", true, "Weekly active accounts"),
        new("Net Revenue Churn", "1.8%", "-0.6%", true, "Lower is better"),
        new("NPS", "61", "+3", true, "Rolling 30-day average")
    ];

    private static readonly IReadOnlyList<SeriesPoint> Revenue =
    [
        new("Jan", 148000), new("Feb", 156000), new("Mar", 171000), new("Apr", 179000),
        new("May", 188000), new("Jun", 196000), new("Jul", 208000), new("Aug", 219000),
        new("Sep", 232000), new("Oct", 249000), new("Nov", 264000), new("Dec", 284200)
    ];

    private static readonly IReadOnlyList<SeriesPoint> Signups =
    [
        new("Mon", 112), new("Tue", 128), new("Wed", 140), new("Thu", 121),
        new("Fri", 154), new("Sat", 118), new("Sun", 132)
    ];

    private static readonly IReadOnlyList<ChannelShare> Channels =
    [
        new("Organic", 36.4m),
        new("Paid", 24.1m),
        new("Partner", 17.3m),
        new("Outbound", 12.5m),
        new("Referral", 9.7m)
    ];

    private static readonly IReadOnlyList<CustomerRow> Customers =
    [
        new("Northstar Logistics", "ops@northstar.io", "Enterprise", "$14,200", "Healthy"),
        new("Pixelmint", "hello@pixelmint.co", "Growth", "$2,980", "At Risk"),
        new("Harbor Retail", "finance@harborretail.com", "Enterprise", "$8,400", "Healthy"),
        new("Loomcare", "team@loomcare.app", "SMB", "$890", "New"),
        new("Zephyr Labs", "admin@zephyrlabs.ai", "Growth", "$3,150", "Healthy")
    ];

    private static readonly IReadOnlyList<OrderRow> Orders =
    [
        new("SO-13092", "Northstar Logistics", "$4,200", "Fulfilled", DateOnly.FromDateTime(DateTime.Today.AddDays(-7))),
        new("SO-13093", "Pixelmint", "$1,340", "Processing", DateOnly.FromDateTime(DateTime.Today.AddDays(-6))),
        new("SO-13094", "Harbor Retail", "$2,780", "Pending", DateOnly.FromDateTime(DateTime.Today.AddDays(-4))),
        new("SO-13095", "Loomcare", "$650", "Cancelled", DateOnly.FromDateTime(DateTime.Today.AddDays(-3))),
        new("SO-13096", "Zephyr Labs", "$1,910", "Fulfilled", DateOnly.FromDateTime(DateTime.Today.AddDays(-1)))
    ];

    private static readonly IReadOnlyList<InvoiceRow> Invoices =
    [
        new("INV-9921", "Northstar Logistics", "$14,200", "Paid", DateOnly.FromDateTime(DateTime.Today.AddDays(12))),
        new("INV-9922", "Harbor Retail", "$8,400", "Due", DateOnly.FromDateTime(DateTime.Today.AddDays(5))),
        new("INV-9923", "Pixelmint", "$2,980", "Overdue", DateOnly.FromDateTime(DateTime.Today.AddDays(-2))),
        new("INV-9924", "Zephyr Labs", "$3,150", "Draft", DateOnly.FromDateTime(DateTime.Today.AddDays(14)))
    ];

    private static readonly IReadOnlyList<TeamMemberRow> TeamMembers =
    [
        new("Mia Hudson", "Head of Operations", "UTC-5", "Online"),
        new("Aria Bennett", "Revenue Analyst", "UTC+1", "Focus"),
        new("Ethan Cole", "Support Manager", "UTC-8", "Online"),
        new("Noah Park", "Product Designer", "UTC+9", "Away"),
        new("Sofia Reed", "Finance Lead", "UTC+0", "Online")
    ];

    private static readonly IReadOnlyList<TaskRow> Tasks =
    [
        new("Review churn report anomalies", "High", "Mia Hudson", DateOnly.FromDateTime(DateTime.Today.AddDays(1)), false),
        new("Finalize Q2 billing policy", "Medium", "Sofia Reed", DateOnly.FromDateTime(DateTime.Today.AddDays(2)), false),
        new("Prepare enterprise onboarding checklist", "Low", "Ethan Cole", DateOnly.FromDateTime(DateTime.Today.AddDays(4)), false),
        new("Ship dashboard palette presets", "Medium", "Noah Park", DateOnly.FromDateTime(DateTime.Today.AddDays(3)), true)
    ];

    private static readonly IReadOnlyList<ActivityRow> Activity =
    [
        new("Mia Hudson", "updated", "Revenue forecast scenario", DateTime.UtcNow.AddMinutes(-14)),
        new("Sofia Reed", "approved", "INV-9924", DateTime.UtcNow.AddMinutes(-42)),
        new("Ethan Cole", "closed", "support ticket #4821", DateTime.UtcNow.AddHours(-2)),
        new("Aria Bennett", "published", "Weekly KPI digest", DateTime.UtcNow.AddHours(-6)),
        new("Noah Park", "commented", "Dashboard redesign brief", DateTime.UtcNow.AddHours(-11))
    ];

    private static readonly IReadOnlyList<AlertRow> Alerts =
    [
        new("Payment retries rising", "Failed retries increased 18% compared to last week.", "warning"),
        new("Large renewal won", "Northstar expanded to 220 seats on annual plan.", "success"),
        new("Trial drop-off", "Activation step 3 completion dipped by 7 points.", "info")
    ];

    public Task<IReadOnlyList<ActivityRow>> GetActivityAsync() => Task.FromResult(Activity);

    public Task<IReadOnlyList<AlertRow>> GetAlertsAsync() => Task.FromResult(Alerts);

    public Task<IReadOnlyList<ChannelShare>> GetChannelSharesAsync() => Task.FromResult(Channels);

    public Task<IReadOnlyList<CustomerRow>> GetCustomersAsync() => Task.FromResult(Customers);

    public Task<IReadOnlyList<InvoiceRow>> GetInvoicesAsync() => Task.FromResult(Invoices);

    public Task<IReadOnlyList<KpiMetric>> GetKpisAsync() => Task.FromResult(Kpis);

    public Task<IReadOnlyList<OrderRow>> GetOrdersAsync() => Task.FromResult(Orders);

    public Task<IReadOnlyList<SeriesPoint>> GetRevenueSeriesAsync() => Task.FromResult(Revenue);

    public Task<IReadOnlyList<SeriesPoint>> GetSignupsSeriesAsync() => Task.FromResult(Signups);

    public Task<IReadOnlyList<TaskRow>> GetTasksAsync() => Task.FromResult(Tasks);

    public Task<IReadOnlyList<TeamMemberRow>> GetTeamMembersAsync() => Task.FromResult(TeamMembers);
}
