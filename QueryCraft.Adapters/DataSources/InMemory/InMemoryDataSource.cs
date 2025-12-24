using QueryCraft.Adapters.Adapters;
using QueryCraft.Adapters.Adapters.Memory;
using QueryCraft.Adapters.Common.Constants;
using QueryCraft.Adapters.Common.Enums;

namespace QueryCraft.Adapters.DataSources.InMemory;

public class InMemoryDataSource : DataSource
{
    public override string FriendlyName => DataSourceNames.InMemoryDataSource;
    public sealed override required object Data { get; init; } = (IEnumerable<object>)[];
    public sealed override DataSourceName DataSourceName { get; set; } = DataSourceName.EnumerableSource;
    public override IDataSourceAdapter Build<T>()
    {
        return new InMemoryDataSourceAdapter<T>(this);
    }
}