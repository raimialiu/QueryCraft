using QueryCraft.Adapters.Adapters;
using QueryCraft.Adapters.Common.Enums;
using QueryCraft.Adapters.EfCore;

namespace QueryCraft.Adapters.DataSources.EfCore;

public class EfCoreDataSource : DataSource
{
    public override string FriendlyName { get; }
    public override required object Data { get; init; }
    public override DataSourceName DataSourceName { get; set; }
    public override IDataSourceAdapter Build<T>()
    {
        return new EfCoreDataSourceAdapters<T>(this);
    }
}