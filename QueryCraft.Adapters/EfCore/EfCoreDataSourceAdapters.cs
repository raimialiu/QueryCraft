using QueryCraft.Adapters.Common.Constants;
using QueryCraft.Adapters.Models;

namespace QueryCraft.Adapters.EfCore;

public class EfCoreDataSourceAdapters<T>(
        DataSource<T> dataSource
    ) : IDataSourceAdapter where T : class
{

    public string AdapterName => AdapterNames.EFCoreDataSource;
    
    public IEnumerable<Type> SupportedTypes { get; }
    
    public bool CanHandle<T1>(DataSource<T1> dataSource)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<T1>> ApplyFilterAsync<T1>(
        DataSource<T1> dataSource, 
        FilterCriteria criteria,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<T1>> ApplyFiltersAsync<T1>(object dataSource, FilterGroup filterGroup, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}