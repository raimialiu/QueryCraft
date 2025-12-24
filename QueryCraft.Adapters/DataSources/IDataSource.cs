using QueryCraft.Adapters.Adapters;

namespace QueryCraft.Adapters.DataSources;

public interface IDataSource
{
    public IDataSourceAdapter Build<T>(); // Build and get the adapter responsible for the underlying data source
}