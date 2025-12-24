using QueryCraft.Adapters.Adapters;
using QueryCraft.Adapters.Common.Constants;
using QueryCraft.Adapters.DataSources.EfCore;
using QueryCraft.Adapters.Models;

namespace QueryCraft.Adapters.EfCore;

public class EfCoreDataSourceAdapters<T>(
        EfCoreDataSource dataSource
    ) : DataSourceAdapter where T : class
{

    public override string AdapterName => AdapterNames.EfCoreDataSource;
    
    public override bool CanHandle<T>()
    {
        throw new NotImplementedException();
    }

    public override Task<QueryResult<T>> ApplyFilterAsync<T>(
        FilterGroup<T> queries, 
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<QueryResult<T>> ApplyFiltersAsync<T>(
        List<FilterGroup<T>> filterGroups, 
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}