using QueryCraft.Adapters.Common.Constants;
using QueryCraft.Adapters.DataSources.InMemory;
using QueryCraft.Adapters.Models;

namespace QueryCraft.Adapters.Adapters.Memory;

public class InMemoryDataSourceAdapter<T>(
        InMemoryDataSource dataSource
    ) : DataSourceAdapter
{
    public override string AdapterName => AdapterNames.EfCoreDataSource;

    public override bool CanHandle<T1>()
    {
        throw new NotImplementedException();
    }

    public override Task<IEnumerable<T1>> ApplyFilterAsync<T1>(
            FilterGroup<T1> queries, 
            CancellationToken cancellationToken = default
        )
    {
        throw new NotImplementedException();
    }

    public override Task<IEnumerable<T1>> ApplyFiltersAsync<T1>(
            List<FilterGroup<T1>> filterGroups, 
            CancellationToken cancellationToken = default
        )
    {
        throw new NotImplementedException();
    }
}