using QueryCraft.Adapters.Common.Constants;
using QueryCraft.Adapters.DataSources.InMemory;
using QueryCraft.Adapters.Models;

namespace QueryCraft.Adapters.Adapters.Memory;

public class InMemoryDataSourceAdapter<T>(
        InMemoryDataSource dataSource
    ) : DataSourceAdapter
{
    public override string AdapterName => AdapterNames.EfCoreDataSource;

    public override bool CanHandle<T>()
    {
        if (dataSource == null) 
            return false;
        
        var dataSourceType = dataSource.GetType();
        return SupportedTypes.Any(supportedType => 
            supportedType.IsAssignableFrom(dataSourceType) ||
            (supportedType.IsGenericTypeDefinition && 
             dataSourceType.IsGenericType &&
             supportedType == dataSourceType.GetGenericTypeDefinition()));
    }

    public override Task<QueryResult<T>> ApplyFilterAsync<T>(
            FilterGroup<T> queries, 
            CancellationToken cancellationToken = default
        )
    {
        throw new NotImplementedException();
    }

    public override Task<QueryResult<T>> ApplyFiltersAsync<T>(
            List<FilterGroup<T>> filterGroups, 
            CancellationToken cancellationToken = default
        )
    {
        throw new NotImplementedException();
    }
}