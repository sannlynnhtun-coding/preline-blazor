namespace PrelineBlazorApp.Models.UI;

public sealed class TableQueryState
{
    public string SearchTerm { get; set; } = string.Empty;
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public IReadOnlyList<int> PageSizeOptions { get; init; } = [5, 10, 20];

    public int GetTotalPages(int filteredCount)
    {
        if (filteredCount <= 0)
        {
            return 1;
        }

        return Math.Max(1, (int)Math.Ceiling(filteredCount / (double)PageSize));
    }

    public void ResetPage() => CurrentPage = 1;

    public void ClampPage(int filteredCount)
    {
        var totalPages = GetTotalPages(filteredCount);
        if (CurrentPage > totalPages)
        {
            CurrentPage = totalPages;
        }

        if (CurrentPage < 1)
        {
            CurrentPage = 1;
        }
    }

    public int GetStartIndex(int filteredCount)
    {
        if (filteredCount <= 0)
        {
            return 0;
        }

        return ((CurrentPage - 1) * PageSize) + 1;
    }

    public int GetEndIndex(int filteredCount)
    {
        if (filteredCount <= 0)
        {
            return 0;
        }

        return Math.Min(CurrentPage * PageSize, filteredCount);
    }
}
