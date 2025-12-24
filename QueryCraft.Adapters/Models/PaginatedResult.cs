namespace QueryCraft.Adapters.Models;

public class PaginatedResult<T>
{
    public IEnumerable<T> Items { get; set; } // Add setter
    public int PageNumber { get; set; } // Add setter
    public int PageSize { get; set; } // Add setter
    public int TotalPages { get; set; } // Add setter
    public int TotalItems { get; set; } // Add setter
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
    
    public PaginatedResult()
    {
        Items = Array.Empty<T>();
    }

    public PaginatedResult(
        IEnumerable<T> items,
        int count,
        int pageNumber,
        int pageSize
        )
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalItems = count;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    }
}