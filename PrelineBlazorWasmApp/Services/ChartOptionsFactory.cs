using System.Text.Json;
using PrelineBlazorWasmApp.Models.Dashboard;

namespace PrelineBlazorWasmApp.Services;

public static class ChartOptionsFactory
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static string CreateRevenueLineOptions(IReadOnlyList<SeriesPoint> points)
    {
        var categories = points.Select(x => x.Label).ToArray();
        var values = points.Select(x => x.Value).ToArray();

        var options = new
        {
            chart = new { backgroundColor = "transparent", spacing = new[] { 12, 12, 12, 12 } },
            title = new { text = "" },
            credits = new { enabled = false },
            xAxis = new { categories },
            yAxis = new { title = new { text = "" } },
            legend = new { enabled = false },
            tooltip = new { shared = true, valuePrefix = "$" },
            series = new[]
            {
                new
                {
                    type = "areaspline",
                    name = "Revenue",
                    data = values,
                    fillOpacity = 0.16,
                    marker = new { enabled = false }
                }
            }
        };

        return JsonSerializer.Serialize(options, SerializerOptions);
    }

    public static string CreateColumnOptions(IReadOnlyList<SeriesPoint> points, string label)
    {
        var options = new
        {
            chart = new { type = "column", backgroundColor = "transparent", spacing = new[] { 12, 12, 12, 12 } },
            title = new { text = "" },
            credits = new { enabled = false },
            legend = new { enabled = false },
            xAxis = new { categories = points.Select(x => x.Label).ToArray(), crosshair = true },
            yAxis = new { min = 0, title = new { text = "" } },
            tooltip = new { valueSuffix = " " + label },
            plotOptions = new { column = new { borderRadius = 6 } },
            series = new[]
            {
                new
                {
                    name = label,
                    data = points.Select(x => x.Value).ToArray()
                }
            }
        };

        return JsonSerializer.Serialize(options, SerializerOptions);
    }

    public static string CreateDonutOptions(IReadOnlyList<ChannelShare> channels)
    {
        var data = channels.Select(x => new object[] { x.Channel, x.Share }).ToArray();

        var options = new
        {
            chart = new { type = "pie", backgroundColor = "transparent" },
            title = new { text = "" },
            credits = new { enabled = false },
            tooltip = new { pointFormat = "<b>{point.y:.1f}%</b>" },
            plotOptions = new
            {
                pie = new
                {
                    innerSize = "68%",
                    borderWidth = 0,
                    dataLabels = new { enabled = true, format = "{point.name}: {point.y:.1f}%" }
                }
            },
            series = new[]
            {
                new
                {
                    name = "Share",
                    data
                }
            }
        };

        return JsonSerializer.Serialize(options, SerializerOptions);
    }

    public static string CreateSparklineOptions(IReadOnlyList<SeriesPoint> points)
    {
        var options = new
        {
            chart = new
            {
                type = "areaspline",
                backgroundColor = "transparent",
                margin = new[] { 2, 0, 2, 0 },
                height = 90,
                animation = false
            },
            title = new { text = "" },
            credits = new { enabled = false },
            xAxis = new { visible = false, categories = points.Select(x => x.Label).ToArray() },
            yAxis = new { visible = false },
            legend = new { enabled = false },
            tooltip = new { outside = true, valueDecimals = 0 },
            series = new[]
            {
                new
                {
                    data = points.Select(x => x.Value).ToArray(),
                    fillOpacity = 0.18,
                    marker = new { enabled = false }
                }
            }
        };

        return JsonSerializer.Serialize(options, SerializerOptions);
    }
}
