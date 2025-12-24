namespace QueryCraft.Adapters.Models;

public class QueryResult<T>
{
    public QueryMetadata Metadata { get; init; }
    public IQueryable<T> Result { get; init; }
}

public class QueryMetadata
{
    public string QueryString { get; set; }
}