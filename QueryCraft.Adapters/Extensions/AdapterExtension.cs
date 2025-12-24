using QueryCraft.Adapters.Common.Constants;
using QueryCraft.Adapters.EfCore;
using QueryCraft.Adapters.Models;

namespace QueryCraft.Adapters.Extensions;

public static class AdapterExtension
{
    public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> queryable, FilterCretia criteria) where T : class
    {
        var dataAdapter = new EfCoreDataSourceAdapters(new DataSource<T>()
        {
            QueryableSource =  queryable,
            FriendlyName = AdapterNames.EfCoreDataSource
        });
    }
}