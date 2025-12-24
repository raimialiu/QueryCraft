using QueryCraft.Adapters.Models;

namespace QueryCraft.Adapters.Adapters;


/// <summary>
/// Defines the contract for data source adapters that provide standardized access
/// to various types of data sources with filtering, querying, and pagination capabilities.
/// </summary>
/// <remarks>
/// <para>
/// This interface implements the Adapter pattern, allowing different data source types
/// (databases, JSON files, in-memory collections, etc.) to be accessed through a
/// consistent API. Each adapter implementation handles the specifics of its data source
/// while providing uniform filtering, querying, and pagination operations.
/// </para>
/// <para>
/// The interface supports both single and multiple filter operations with asynchronous
/// execution, making it suitable for high-performance applications that need to work
/// with various data sources without tight coupling to specific implementations.
/// </para>
/// <para>
/// All query operations return <see cref="QueryResult{T}"/> objects that include both
/// the filtered data and metadata about the query execution, providing transparency
/// and debugging capabilities.
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
///         Condition = FilterCondition.And,
///         Queries = new List&lt;FilterQuery&lt;User&gt;&gt;
///         {
///             new FilterQuery&lt;User&gt;
///             {
///                 Query = new FilterCriteria&lt;int&gt;(new[] { 18 }, FilterOperator.GREATER_THAN, "Age"),
///                 Condition = FilterCondition.And
///             }
///         },
///         PaginationQuery = new PaginationQuery { PageSize = 10, PageNumber = 1 }
///     };
///     
///     var queryResult = await adapter.ApplyFilterAsync(filterGroup, cancellationToken);
///     
///     // Access the filtered data
///     var users = queryResult.Result.Data;
///     
///     // Access query metadata
///     var executedQuery = queryResult.Metadata.QueryString;
///     var totalCount = queryResult.Result.TotalCount;
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
    ///     var queryResult = await adapter.ApplyFilterAsync&lt;User&gt;(filterGroup);
    /// }
    /// </code>
    /// </example>
    bool CanHandle<T>();
    
    /// <summary>
    /// Asynchronously applies a single filter group to the data source and returns the filtered results
    /// with query metadata and optional pagination.
    /// </summary>
    /// <typeparam name="T">The type of objects to filter and return.</typeparam>
    /// <param name="queries">
    /// The filter group containing one or more filter queries to apply to the data source,
    /// with optional pagination configuration.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used to cancel the asynchronous operation.
    /// Defaults to <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous filtering operation.
    /// The task result contains a <see cref="QueryResult{T}"/> with both the filtered data
    /// and metadata about the query execution.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method applies all filter queries within the provided <paramref name="queries"/> group
    /// using the logical conditions specified in each query and the group's overall condition.
    /// The implementation should respect the filter group's combination logic (AND/OR operations).
    /// </para>
    /// <para>
    /// If pagination is specified in the filter group, it will be applied after filtering
    /// to limit the result set. The total count of matching records (before pagination)
    /// should be included in the result metadata.
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
    /// // Create a filter group for active users over 18 with pagination
    /// var filterGroup = new FilterGroup&lt;User&gt;
    /// {
    ///     Condition = FilterCondition.And,
    ///     Queries = new List&lt;FilterQuery&lt;User&gt;&gt;
    ///     {
    ///         new FilterQuery&lt;User&gt;
    ///         {
    ///             Query = new FilterCriteria&lt;int&gt;(new[] { 18 }, FilterOperator.GREATER_THAN, "Age"),
    ///             Condition = FilterCondition.And
    ///         },
    ///         new FilterQuery&lt;User&gt;
    ///         {
    ///             Query = new FilterCriteria&lt;bool&gt;(new[] { true }, FilterOperator.EQUALS, "IsActive"),
    ///             Condition = FilterCondition.And
    ///         }
    ///     },
    ///     PaginationQuery = new PaginationQuery { PageSize = 10, PageNumber = 1 }
    /// };
    /// 
    /// // Apply the filter
    /// var queryResult = await adapter.ApplyFilterAsync(filterGroup, cancellationToken);
    /// 
    /// // Access the results
    /// var users = queryResult.Result.Data;
    /// var totalCount = queryResult.Result.TotalCount;
    /// var executedQuery = queryResult.Metadata.QueryString;
    /// 
    /// Console.WriteLine($"Found {totalCount} total users, showing page 1 of {Math.Ceiling((double)totalCount / 10)}");
    /// Console.WriteLine($"Executed query: {executedQuery}");
    /// 
    /// foreach (var user in users)
    /// {
    ///     Console.WriteLine($"User: {user.Name}, Age: {user.Age}");
    /// }
    /// </code>
    /// </example>
    Task<QueryResult<T>> ApplyFilterAsync<T>(
        FilterGroup<T> queries,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Asynchronously applies multiple filter groups to the data source with complex logical operations
    /// and returns the combined filtered results with query metadata and optional pagination.
    /// </summary>
    /// <typeparam name="T">The type of objects to filter and return.</typeparam>
    /// <param name="filterGroups">
    /// A list of filter groups, each containing multiple filter queries.
    /// The groups are combined using logical operators to create complex query conditions.
    /// Pagination, if specified, should be consistent across all groups.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used to cancel the asynchronous operation.
    /// Defaults to <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous filtering operation.
    /// The task result contains a <see cref="QueryResult{T}"/> with both the combined filtered data
    /// and metadata about the query execution.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method is designed for complex filtering scenarios where multiple independent
    /// filter groups need to be combined with logical operators (AND/OR) between groups.
    /// Each filter group is processed according to its internal condition configuration,
    /// and then the results are combined based on the inter-group relationships.
    /// </para>
    /// <para>
    /// The method provides more flexibility than <see cref="ApplyFilterAsync{T}(FilterGroup{T}, CancellationToken)"/>
    /// by allowing complex query structures such as: (Group1 AND Group2) OR (Group3 AND Group4).
    /// </para>
    /// <para>
    /// If any filter group specifies pagination, the pagination parameters should be consistent
    /// across all groups. The implementation should apply pagination to the final combined result set.
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
    /// Thrown when <paramref name="filterGroups"/> is empty, contains null elements,
    /// or has inconsistent pagination parameters across groups.
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
    /// // Create multiple filter groups for complex querying with pagination
    /// var filterGroups = new List&lt;FilterGroup&lt;User&gt;&gt;
    /// {
    ///     // Group 1: Active adult users
    ///     new FilterGroup&lt;User&gt;
    ///     {
    ///         Condition = FilterCondition.And,
    ///         Queries = new List&lt;FilterQuery&lt;User&gt;&gt;
    ///         {
    ///             new FilterQuery&lt;User&gt;
    ///             {
    ///                 Query = new FilterCriteria&lt;int&gt;(new[] { 18 }, FilterOperator.GREATER_THAN_OR_EQUAL, "Age"),
    ///                 Condition = FilterCondition.And
    ///             },
    ///             new FilterQuery&lt;User&gt;
    ///             {
    ///                 Query = new FilterCriteria&lt;bool&gt;(new[] { true }, FilterOperator.EQUALS, "IsActive"),
    ///                 Condition = FilterCondition.And
    ///             }
    ///         },
    ///         PaginationQuery = new PaginationQuery { PageSize = 20, PageNumber = 1 }
    ///     },
    ///     // Group 2: VIP users regardless of age
    ///     new FilterGroup&lt;User&gt;
    ///     {
    ///         Condition = FilterCondition.And,
    ///         Queries = new List&lt;FilterQuery&lt;User&gt;&gt;
    ///         {
    ///             new FilterQuery&lt;User&gt;
    ///             {
    ///                 Query = new FilterCriteria&lt;bool&gt;(new[] { true }, FilterOperator.EQUALS, "IsVip"),
    ///                 Condition = FilterCondition.And
    ///             },
    ///             new FilterQuery&lt;User&gt;
    ///             {
    ///                 Query = new FilterCriteria&lt;bool&gt;(new[] { true }, FilterOperator.EQUALS, "IsActive"),
    ///                 Condition = FilterCondition.And
    ///             }
    ///         },
    ///         PaginationQuery = new PaginationQuery { PageSize = 20, PageNumber = 1 }
    ///     }
    /// };
    /// 
    /// // Apply multiple filter groups (results will be: Group1 OR Group2)
    /// var queryResult = await adapter.ApplyFiltersAsync(filterGroups, cancellationToken);
    /// 
    /// // Access the results
    /// var users = queryResult.Result.Data;
    /// var totalCount = queryResult.Result.TotalCount;
    /// var executedQuery = queryResult.Metadata.QueryString;
    /// 
    /// Console.WriteLine($"Found {totalCount} total users matching complex criteria");
    /// Console.WriteLine($"Executed query: {executedQuery}");
    /// Console.WriteLine($"Showing {users.Count()} users on current page");
    /// </code>
    /// </example>
    Task<QueryResult<T>> ApplyFiltersAsync<T>(
        List<FilterGroup<T>> filterGroups,
        CancellationToken cancellationToken = default);
}