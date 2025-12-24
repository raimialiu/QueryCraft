using QueryCraft.Adapters.Adapters;
using QueryCraft.Adapters.Adapters.Memory;
using QueryCraft.Adapters.Common.Constants;
using QueryCraft.Adapters.Common.Enums;

namespace QueryCraft.Adapters.DataSources.InMemory;

/// <summary>
/// Provides an in-memory data source implementation that works with enumerable collections
/// stored in memory for fast access and filtering operations.
/// </summary>
/// <remarks>
/// <para>
/// This data source is optimized for scenarios where data is already loaded in memory
/// and needs to be filtered using LINQ operations. It's particularly useful for:
/// <list type="bullet">
/// <item><description>Small to medium-sized datasets that fit comfortably in memory</description></item>
/// <item><description>Cached data that needs frequent filtering operations</description></item>
/// <item><description>Test scenarios where you need predictable, fast data access</description></item>
/// <item><description>Configuration data or lookup tables</description></item>
/// </list>
/// </para>
/// <para>
/// The class uses <see cref="DataSourceName.EnumerableSource"/> as its data source type,
/// indicating that it works with <see cref="IEnumerable{T}"/> collections and applies
/// filters using LINQ-to-Objects operations.
/// </para>
/// <para>
/// Performance characteristics:
/// <list type="bullet">
/// <item><description>Very fast for small datasets (under 10,000 records)</description></item>
/// <item><description>Good performance for medium datasets with proper indexing</description></item>
/// <item><description>Memory usage scales linearly with data size</description></item>
/// <item><description>No I/O overhead since data is already in memory</description></item>
/// </list>
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Create with a list of users
/// var users = new List&lt;User&gt;
/// {
///     new User { Id = 1, Name = "John", Age = 25, IsActive = true },
///     new User { Id = 2, Name = "Jane", Age = 30, IsActive = true },
///     new User { Id = 3, Name = "Bob", Age = 35, IsActive = false }
/// };
/// 
/// var dataSource = new InMemoryDataSource
/// {
///     Data = users
/// };
/// 
/// // Build adapter and use for filtering
/// var adapter = dataSource.Build&lt;User&gt;();
/// 
/// var filterGroup = new FilterGroup&lt;User&gt;
/// {
///     Condition = FilterCondition.And,
///     Queries = new List&lt;FilterQuery&lt;User&gt;&gt;
///     {
///         new FilterQuery&lt;User&gt;
///         {
///             Query = new FilterCriteria&lt;bool&gt;(new[] { true }, FilterOperator.EQUALS, "IsActive"),
///             Condition = FilterCondition.And
///         }
///     }
/// };
/// 
/// var result = await adapter.ApplyFilterAsync(filterGroup);
/// // Returns John and Jane (active users)
/// </code>
/// </example>
public class InMemoryDataSource : DataSource
{
    /// <summary>
    /// Gets the friendly display name for this data source type.
    /// </summary>
    /// <value>
    /// Returns the constant value from <see cref="DataSourceNames.InMemoryDataSource"/>,
    /// which provides a standardized display name for in-memory data sources.
    /// </value>
    /// <remarks>
    /// <para>
    /// This property provides a consistent, user-friendly name that can be displayed
    /// in user interfaces, logs, and configuration screens. The value is retrieved
    /// from a constants class to ensure consistency across the application.
    /// </para>
    /// <para>
    /// The friendly name is typically something like "In-Memory Collection" or
    /// "Memory Data Source" and is used for identification purposes.
    /// </para>
    /// </remarks>
    public override string FriendlyName => DataSourceNames.InMemoryDataSource;

    /// <summary>
    /// Gets or sets the enumerable collection that serves as the data source.
    /// </summary>
    /// <value>
    /// An <see cref="IEnumerable{Object}"/> collection containing the data to be filtered.
    /// Defaults to an empty enumerable collection.
    /// </value>
    /// <remarks>
    /// <para>
    /// This property must be set during object initialization and contains the actual
    /// data that will be filtered by the adapter. The data should be an enumerable
    /// collection of objects that can be cast to the target type when filtering.
    /// </para>
    /// <para>
    /// The <c>sealed</c> modifier prevents derived classes from overriding this property,
    /// ensuring consistent behavior for in-memory data sources. The <c>required</c>
    /// modifier ensures that data must be provided during object construction.
    /// </para>
    /// <para>
    /// Expected data types include:
    /// <list type="bullet">
    /// <item><description><c>List&lt;T&gt;</c> - Most common, provides indexed access</description></item>
    /// <item><description><c>T[]</c> - Arrays for fixed-size collections</description></item>
    /// <item><description><c>IQueryable&lt;T&gt;</c> - For deferred execution scenarios</description></item>
    /// <item><description><c>ICollection&lt;T&gt;</c> - For collections with count information</description></item>
    /// <item><description>Any <c>IEnumerable&lt;T&gt;</c> implementation</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <exception cref="System.ArgumentNullException">
    /// May be thrown by the adapter if null data is provided and the adapter
    /// cannot handle null collections.
    /// </exception>
    /// <exception cref="System.InvalidCastException">
    /// May be thrown during filtering operations if the data cannot be cast
    /// to the expected type specified in the filter operations.
    /// </exception>
    /// <example>
    /// <code>
    /// // With strongly-typed list
    /// var dataSource = new InMemoryDataSource
    /// {
    ///     Data = new List&lt;User&gt; { /* users */ }
    /// };
    /// 
    /// // With array
    /// var dataSource = new InMemoryDataSource
    /// {
    ///     Data = new User[] { /* users */ }
    /// };
    /// 
    /// // With LINQ query (deferred execution)
    /// var dataSource = new InMemoryDataSource
    /// {
    ///     Data = existingUsers.Where(u => u.IsActive)
    /// };
    /// 
    /// // With mixed object types (requires careful filtering)
    /// var dataSource = new InMemoryDataSource
    /// {
    ///     Data = new object[] { user1, user2, product1 }
    /// };
    /// </code>
    /// </example>
    public sealed override required object Data { get; init; } = (IEnumerable<object>)[];

    /// <summary>
    /// Gets or sets the data source name that categorizes this data source type.
    /// </summary>
    /// <value>
    /// Always returns <see cref="DataSourceName.EnumerableSource"/>, indicating that
    /// this data source works with enumerable collections and LINQ-to-Objects operations.
    /// </value>
    /// <remarks>
    /// <para>
    /// This property is sealed to prevent derived classes from changing the data source
    /// type, ensuring that in-memory data sources are always categorized as enumerable sources.
    /// This categorization affects how the adapter processes queries and applies filters.
    /// </para>
    /// <para>
    /// The <see cref="DataSourceName.EnumerableSource"/> designation means:
    /// <list type="bullet">
    /// <item><description>Filters are applied using LINQ-to-Objects</description></item>
    /// <item><description>All operations happen in memory</description></item>
    /// <item><description>No database or external I/O operations</description></item>
    /// <item><description>Performance is limited by available memory and CPU</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public sealed override DataSourceName DataSourceName { get; set; } = DataSourceName.EnumerableSource;

    /// <summary>
    /// Creates and returns an in-memory data source adapter configured for the specified type.
    /// </summary>
    /// <typeparam name="T">
    /// The type of objects that the adapter will filter. This type should match or be
    /// compatible with the objects in the <see cref="Data"/> collection.
    /// </typeparam>
    /// <returns>
    /// An <see cref="IDataSourceAdapter"/> implementation specifically designed for
    /// in-memory filtering operations on type <typeparamref name="T"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method implements the Builder pattern by creating an appropriate adapter
    /// instance based on the current data source configuration. The returned adapter
    /// is specifically optimized for in-memory operations using LINQ-to-Objects.
    /// </para>
    /// <para>
    /// The generic type parameter <typeparamref name="T"/> should match the actual
    /// type of objects stored in the <see cref="Data"/> collection. If there's a type
    /// mismatch, filtering operations may fail with cast exceptions.
    /// </para>
    /// <para>
    /// The created adapter will:
    /// <list type="bullet">
    /// <item><description>Cast the data to <c>IEnumerable&lt;T&gt;</c> for type safety</description></item>
    /// <item><description>Apply filters using LINQ Where clauses</description></item>
    /// <item><description>Handle pagination using Skip and Take operations</description></item>
    /// <item><description>Generate readable query strings for debugging</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <exception cref="System.InvalidCastException">
    /// May be thrown if the <see cref="Data"/> collection contains objects that
    /// cannot be cast to type <typeparamref name="T"/>.
    /// </exception>
    /// <exception cref="System.ArgumentException">
    /// May be thrown if the data source is not properly configured or if the
    /// data is in an unexpected format.
    /// </exception>
    /// <example>
    /// <code>
    /// // Create data source with users
    /// var users = new List&lt;User&gt;
    /// {
    ///     new User { Name = "John", Age = 25 },
    ///     new User { Name = "Jane", Age = 30 }
    /// };
    /// 
    /// var dataSource = new InMemoryDataSource { Data = users };
    /// 
    /// // Build adapter for User type
    /// var adapter = dataSource.Build&lt;User&gt;();
    /// 
    /// // Verify adapter can handle User type
    /// if (adapter.CanHandle&lt;User&gt;())
    /// {
    ///     // Create and apply filters
    ///     var filterGroup = new FilterGroup&lt;User&gt; { /* filter configuration */ };
    ///     var result = await adapter.ApplyFilterAsync(filterGroup);
    ///     
    ///     Console.WriteLine($"Adapter: {adapter.AdapterName}");
    ///     Console.WriteLine($"Query: {result.Metadata.QueryString}");
    ///     Console.WriteLine($"Results: {result.Result.Data.Count()}");
    /// }
    /// 
    /// // Example with type mismatch (will cause issues)
    /// var productData = new List&lt;Product&gt; { /* products */ };
    /// var productSource = new InMemoryDataSource { Data = productData };
    /// var userAdapter = productSource.Build&lt;User&gt;(); // Type mismatch!
    /// // Filtering operations will fail with cast exceptions
    /// </code>
    /// </example>
    public override IDataSourceAdapter Build<T>()
    {
        return new InMemoryDataSourceAdapter<T>(this);
    }
}