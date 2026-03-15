namespace PrelineBlazorApp.Models.Dashboard;

public sealed record KpiMetric(string Label, string Value, string Delta, bool DeltaPositive, string Hint);

public sealed record SeriesPoint(string Label, decimal Value);

public sealed record ChannelShare(string Channel, decimal Share);

public sealed record CustomerRow(string Name, string Email, string Segment, string Mrr, string Status);

public sealed record OrderRow(string OrderNo, string Customer, string Amount, string Status, DateOnly CreatedOn);

public sealed record InvoiceRow(string Number, string Customer, string Amount, string State, DateOnly DueOn);

public sealed record TeamMemberRow(string Name, string Role, string Timezone, string Status);

public sealed record TaskRow(string Title, string Priority, string Assignee, DateOnly DueOn, bool Done);

public sealed record ActivityRow(string Actor, string Action, string Target, DateTime OccurredAtUtc);

public sealed record AlertRow(string Title, string Message, string Severity);
