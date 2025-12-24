namespace QueryCraft.Adapters.Models;

public class DataSource<T>
{
    public string FriendlyName { get; init; } = null!;
    public IEnumerable<T> EnumerableSource { get; init; }
    public IQueryable QueryableSource { get; init; }
}