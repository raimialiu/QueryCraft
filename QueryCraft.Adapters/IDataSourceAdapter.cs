using QueryCraft.Adapters.Models;

namespace QueryCraft.Adapters;

public interface IDataSourceAdapter
{
    /// <summary>
    /// Gets the unique name identifier for this adapter
    /// </summary>
    string AdapterName { get; }
    
    /// <summary>
    /// Gets the collection of data source types this adapter supports
    /// </summary>
    IEnumerable<Type> SupportedTypes { get; }
    
    /// <summary>
    /// Determines if this adapter can handle the specified data source
    /// </summary>
    /// <param name="dataSource">The data source to check</param>
    /// <returns>True if the adapter can handle the data source</returns>
    bool CanHandle<T>(DataSource<T> dataSource);
    
    /// <summary>
    /// Applies a single filter criteria to the data source
    /// </summary>
    Task<IEnumerable<T>> ApplyFilterAsync<T>(
        DataSource<T> dataSource,
        FilterCriteria criteria,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Applies multiple filter criteria with logical operators
    /// </summary>
    Task<IEnumerable<T>> ApplyFiltersAsync<T>(
        object dataSource,
        FilterGroup filterGroup,
        CancellationToken cancellationToken = default);
}