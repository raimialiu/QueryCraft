using QueryCraft.Adapters.Models;

namespace QueryCraft.Adapters.Adapters;

public abstract class DataSourceAdapter : IDataSourceAdapter
{
    public abstract string AdapterName { get; }
    public virtual IEnumerable<Type> SupportedTypes { get; } = [];
    
    public abstract bool CanHandle<T>();

    public abstract Task<QueryResult<T>> ApplyFilterAsync<T>(
        FilterGroup<T> queries,
        CancellationToken cancellationToken = default
    );

    public abstract Task<QueryResult<T>> ApplyFiltersAsync<T>(
        List<FilterGroup<T>> filterGroups, CancellationToken cancellationToken = default);
}