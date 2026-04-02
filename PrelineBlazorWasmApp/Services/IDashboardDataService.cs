using PrelineBlazorWasmApp.Models.Dashboard;

namespace PrelineBlazorWasmApp.Services;

public interface IDashboardDataService
{
    Task<IReadOnlyList<KpiMetric>> GetKpisAsync();
    Task<IReadOnlyList<SeriesPoint>> GetRevenueSeriesAsync();
    Task<IReadOnlyList<SeriesPoint>> GetSignupsSeriesAsync();
    Task<IReadOnlyList<ChannelShare>> GetChannelSharesAsync();
    Task<IReadOnlyList<CustomerRow>> GetCustomersAsync();
    Task<IReadOnlyList<OrderRow>> GetOrdersAsync();
    Task<IReadOnlyList<InvoiceRow>> GetInvoicesAsync();
    Task<IReadOnlyList<TeamMemberRow>> GetTeamMembersAsync();
    Task<IReadOnlyList<TaskRow>> GetTasksAsync();
    Task<IReadOnlyList<ActivityRow>> GetActivityAsync();
    Task<IReadOnlyList<AlertRow>> GetAlertsAsync();
}
