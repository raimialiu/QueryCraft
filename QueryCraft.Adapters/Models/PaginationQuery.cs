namespace QueryCraft.Adapters.Models;

public class PaginationQuery
{
    public PaginationQuery()
    {
        SortBy = String.Empty;
    }
    public PaginationQuery(
        int? currentPage, 
        int? pageSize, 
        string? sortBy, 
        string? thenBy,
        bool isDescending = true
    )
    {
        CurrentPage = currentPage;
        ThenBy = thenBy;
        PageSize = pageSize;
        SortBy = sortBy;
        IsDescending = isDescending;
    }
    public int? CurrentPage { get; set; }
    public int? PageSize { get; set; }
    public string? SortBy { get; set; } // | seperated column
    public string? ThenBy { get; set; } // pipe seperated columns
    public bool IsDescending { get; set; }
}