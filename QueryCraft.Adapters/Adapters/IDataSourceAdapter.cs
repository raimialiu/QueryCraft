using QueryCraft.Adapters.Models;

namespace QueryCraft.Adapters.Adapters;

/// <summary>
/// Defines the contract for data source adapters that provide standardized access
/// to various types of data sources with filtering and querying capabilities.
/// </summary>
/// <remarks>
/// <para>
/// This interface implements the Adapter pattern, allowing different data source types
/// (databases, JSON files, in-memory collections, etc.) to be accessed through a
/// consistent API. Each adapter implementation handles the specifics of its data source
/// while providing uniform filtering and querying operations.
/// </para>
/// <para>
/// The interface supports both single and multiple filter operations with asynchronous
/// execution, making it suitable for high-performance applications that need to work
/// with various data sources without tight coupling to specific implementations.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Usage example
/// IDataSourceAdapter adapter = dataSource.Build();
/// 
/// if (adapter.CanHandle&lt;User&gt;())
/// {
///     var filterGroup = new FilterGroup&lt;User&gt;
///     {
///         Filters = new[] { new Filter&lt;User&gt;(u => u.Age > 18) }
///     };
///     
///     var results = await adapter.ApplyFilterAsync(filterGroup, cancellationToken);
/// }
/// </code>
/// </example>
public interface IDataSourceAdapter
{
    /// <summary>
    /// Gets the unique name identifier for this adapter implementation.
    /// </summary>
    /// <value>
    /// A string that uniquely identifies the adapter type and implementation.
    /// </value>
    /// <remarks>
    /// <para>
    /// This name is used for logging, debugging, configuration, and adapter selection.
    /// It should be descriptive and unique across all adapter implementations in the system.
    /// </para>
    /// <para>
    /// Common naming conventions include the data source type and adapter purpose,
    /// such as "SqlServerAdapter", "JsonFileAdapter", or "InMemoryCollectionAdapter".
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// public string AdapterName => "EntityFrameworkAdapter";
    /// public string AdapterName => "JsonFileAdapter";
    /// public string AdapterName => "InMemoryCollectionAdapter";
    /// </code>
    /// </example>
    string AdapterName { get; }
    
    /// <summary>
    /// Gets the collection of .NET types that this adapter can process and filter.
    /// </summary>
    /// <value>
    /// An enumerable collection of Type objects representing all data types
    /// that this adapter implementation supports.
    /// </value>
    /// <remarks>
    /// <para>
    /// This property is used for type validation before attempting filtering operations.
    /// It helps prevent runtime errors by allowing callers to verify compatibility
    /// before executing queries.
    /// </para>
    /// <para>
    /// The supported types should align with the underlying data source capabilities.
    /// For example, a database adapter might support primitive types and entities,
    /// while a JSON adapter might support dictionaries and dynamic objects.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Database adapter example
    /// public IEnumerable&lt;Type&gt; SupportedTypes => new[]
    /// {
    ///     typeof(string), typeof(int), typeof(DateTime),
    ///     typeof(User), typeof(Product), typeof(Order)
    /// };
    /// 
    /// // JSON adapter example
    /// public IEnumerable&lt;Type&gt; SupportedTypes => new[]
    /// {
    ///     typeof(object), typeof(Dictionary&lt;string, object&gt;),
    ///     typeof(JObject), typeof(dynamic)
    /// };
    /// </code>
    /// </example>
    IEnumerable<Type> SupportedTypes { get; }
    
    /// <summary>
    /// Determines whether this adapter can handle operations for the specified generic type.
    /// </summary>
    /// <typeparam name="T">The type to check for compatibility with this adapter.</typeparam>
    /// <returns>
    /// <c>true</c> if the adapter can process data of type <typeparamref name="T"/>;
    /// otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method performs type compatibility checking before attempting any data operations.
    /// It should verify that the specified type <typeparamref name="T"/> is included in the
    /// <see cref="SupportedTypes"/> collection or can be handled by the adapter's implementation.
    /// </para>
    /// <para>
    /// The method enables compile-time type safety while providing runtime validation,
    /// preventing invalid operations that could result in exceptions during filtering.
    /// </para>
    /// <para>
    /// Implementations should consider inheritance hierarchies and interface implementations
    /// when determining compatibility, not just exact type matches.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Basic implementation
    /// public bool CanHandle&lt;T&gt;()
    /// {
    ///     return SupportedTypes.Contains(typeof(T)) || 
    ///            SupportedTypes.Any(type => type.IsAssignableFrom(typeof(T)));
    /// }
    /// 
    /// // Usage
    /// if (adapter.CanHandle&lt;User&gt;())
    /// {
    ///     // Safe to proceed with User operations
    ///     var results = await adapter.ApplyFilterAsync&lt;User&gt;(filterGroup);
    /// }
    /// </code>
    /// </example>
    bool CanHandle<T>();
    
    /// <summary>
    /// Asynchronously applies a single filter group to the data source and returns the filtered results.
    /// </summary>
    /// <typeparam name="T">The type of objects to filter and return.</typeparam>
    /// <param name="queries">
    /// The filter group containing one or more filter criteria to apply to the data source.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used to cancel the asynchronous operation.
    /// Defaults to <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous filtering operation.
    /// The task result contains an enumerable collection of objects of type <typeparamref name="T"/>
    /// that match the specified filter criteria.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method applies all filters within the provided <paramref name="queries"/> group
    /// using the logical operators specified in the filter group configuration.
    /// The implementation should respect the filter group's combination logic (AND/OR operations).
    /// </para>
    /// <para>
    /// The method is designed for scenarios where a single set of related filter criteria
    /// needs to be applied. For multiple independent filter groups, use
    /// <see cref="ApplyFiltersAsync{T}(List{FilterGroup{T}}, CancellationToken)"/> instead.
    /// </para>
    /// <para>
    /// Implementations should handle the cancellation token appropriately, especially
    /// for long-running operations or when working with remote data sources.
    /// </para>
    /// </remarks>
    /// <exception cref="System.ArgumentNullException">
    /// Thrown when <paramref name="queries"/> is null.
    /// </exception>
    /// <exception cref="System.NotSupportedException">
    /// Thrown when the adapter cannot handle type <typeparamref name="T"/> or
    /// when the filter criteria are not supported by the underlying data source.
    /// </exception>
    /// <exception cref="System.OperationCanceledException">
    /// Thrown when the operation is cancelled via the <paramref name="cancellationToken"/>.
    /// </exception>
    /// <example>
    /// <code>
    /// // Create a filter group for users over 18
    /// var filterGroup = new FilterGroup&lt;User&gt;
    /// {
    ///     LogicalOperator = LogicalOperator.And,
    ///     Filters = new[]
    ///     {
    ///         new Filter&lt;User&gt;(u => u.Age > 18),
    ///         new Filter&lt;User&gt;(u => u.IsActive == true)
    ///     }
    /// };
    /// 
    /// // Apply the filter
    /// var results = await adapter.ApplyFilterAsync(filterGroup, cancellationToken);
    /// 
    /// foreach (var user in results)
    /// {
    ///     Console.WriteLine($"User: {user.Name}, Age: {user.Age}");
    /// }
    /// </code>
    /// </example>
    Task<IEnumerable<T>> ApplyFilterAsync<T>(
        FilterGroup<T> queries,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Asynchronously applies multiple filter groups to the data source with complex logical operations
    /// and returns the combined filtered results.
    /// </summary>
    /// <typeparam name="T">The type of objects to filter and return.</typeparam>
    /// <param name="filterGroups">
    /// A list of filter groups, each containing multiple filter criteria.
    /// The groups are combined using logical operators to create complex query conditions.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used to cancel the asynchronous operation.
    /// Defaults to <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous filtering operation.
    /// The task result contains an enumerable collection of objects of type <typeparamref name="T"/>
    /// that match the combined filter criteria from all filter groups.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method is designed for complex filtering scenarios where multiple independent
    /// filter groups need to be combined with logical operators (AND/OR) between groups.
    /// Each filter group is processed according to its internal logical operator configuration,
    /// and then the results are combined based on the inter-group relationships.
    /// </para>
    /// <para>
    /// The method provides more flexibility than <see cref="ApplyFilterAsync{T}(FilterGroup{T}, CancellationToken)"/>
    /// by allowing complex query structures such as: (Group1 AND Group2) OR (Group3 AND Group4).
    /// </para>
    /// <para>
    /// Implementations should optimize the query execution by leveraging the underlying
    /// data source's native querying capabilities when possible, rather than applying
    /// filters in memory after data retrieval.
    /// </para>
    /// </remarks>
    /// <exception cref="System.ArgumentNullException">
    /// Thrown when <paramref name="filterGroups"/> is null.
    /// </exception>
    /// <exception cref="System.ArgumentException">
    /// Thrown when <paramref name="filterGroups"/> is empty or contains null elements.
    /// </exception>
    /// <exception cref="System.NotSupportedException">
    /// Thrown when the adapter cannot handle type <typeparamref name="T"/> or
    /// when the filter criteria are not supported by the underlying data source.
    /// </exception>
    /// <exception cref="System.OperationCanceledException">
    /// Thrown when the operation is cancelled via the <paramref name="cancellationToken"/>.
    /// </exception>
    /// <example>
    /// <code>
    /// // Create multiple filter groups for complex querying
    /// var filterGroups = new List&lt;FilterGroup&lt;User&gt;&gt;
    /// {
    ///     // Group 1: Active adult users
    ///     new FilterGroup&lt;User&gt;
    ///     {
    ///         LogicalOperator = LogicalOperator.And,
    ///         Filters = new[]
    ///         {
    ///             new Filter&lt;User&gt;(u => u.Age >= 18),
    ///             new Filter&lt;User&gt;(u => u.IsActive == true)
    ///         }
    ///     },
    ///     // Group 2: VIP users regardless of age
    ///     new FilterGroup&lt;User&gt;
    ///     {
    ///         LogicalOperator = LogicalOperator.And,
    ///         Filters = new[]
    ///         {
    ///             new Filter&lt;User&gt;(u => u.IsVip == true),
    ///             new Filter&lt;User&gt;(u => u.IsActive == true)
    ///         }
    ///     }
    /// };
    /// 
    /// // Apply multiple filter groups (results will be: Group1 OR Group2)
    /// var results = await adapter.ApplyFiltersAsync(filterGroups, cancellationToken);
    /// 
    /// Console.WriteLine($"Found {results.Count()} users matching the criteria");
    /// </code>
    /// </example>
    Task<IEnumerable<T>> ApplyFiltersAsync<T>(
        List<FilterGroup<T>> filterGroups,
        CancellationToken cancellationToken = default);
}