using QueryCraft.Adapters.Adapters;
using QueryCraft.Adapters.Common.Enums;

namespace QueryCraft.Adapters.DataSources;

/// <summary>
/// Abstract base class that defines the contract for all data source implementations.
/// Provides a standardized interface for handling various types of data sources
/// while enforcing implementation of core properties in derived classes.
/// </summary>
/// <remarks>
/// This class follows the Template Method pattern, defining the overall structure
/// of a data source while allowing derived classes to provide specific implementations.
/// All derived classes must implement the abstract properties and can optionally
/// override the virtual properties to customize behavior.
/// </remarks>
/// <example>
/// <code>
/// public class DatabaseDataSource : DataSource
/// {
///     public override string FriendlyName { get; init; } = "SQL Database";
///     public override required object Data { get; init; }
///     
///     public override IEnumerable&lt;Type&gt; SupportedTypes { get; set; } = 
///         new[] { typeof(string), typeof(int), typeof(DateTime) };
/// }
/// 
/// // Usage
/// var dbSource = new DatabaseDataSource 
/// { 
///     Data = connectionString,
///     DataSourceName = DataSourceName.DbContextSource
/// };
/// </code>
/// </example>
public abstract class DataSource : IDataSource
{
    /// <summary>
    /// Gets a human-readable name for the data source that will be displayed in user interfaces.
    /// </summary>
    /// <value>
    /// A descriptive string that identifies this data source to end users.
    /// </value>
    /// <remarks>
    /// This property must be implemented by all derived classes to provide a meaningful
    /// display name. The value is immutable after object construction.
    /// </remarks>
    public abstract string FriendlyName { get;  }

    /// <summary>
    /// Gets the actual data content managed by this data source.
    /// </summary>
    /// <value>
    /// The data object that this data source encapsulates. Can be any type.
    /// </value>
    /// <remarks>
    /// <para>
    /// This property must be implemented by all derived classes and must be provided
    /// during object construction due to the required modifier.
    /// </para>
    /// <para>
    /// The use of object type provides maximum flexibility but sacrifices compile-time
    /// type safety. Consider using generics if type safety is important for your use case.
    /// </para>
    /// <para>
    /// Expected data types based on DataSourceName:
    /// <list type="bullet">
    /// <item><description>QueryableSource: IQueryable&lt;T&gt; implementations</description></item>
    /// <item><description>EnumerableSource: IEnumerable&lt;T&gt; collections</description></item>
    /// <item><description>DbContextSource: DbContext instances or connection strings</description></item>
    /// <item><description>JsonSource: JSON strings, file paths, or deserialized objects</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <exception cref="System.ArgumentNullException">
    /// Thrown if null is assigned during construction (implementation dependent).
    /// </exception>
    public abstract required object Data { get; init; }

    /// <summary>
    /// Gets or sets the identifier that specifies the type or category of this data source.
    /// </summary>
    /// <value>
    /// A <see cref="DataSourceName"/> enum value that categorizes this data source.
    /// </value>
    /// <remarks>
    /// <para>
    /// This property can be modified after object creation and is used to identify
    /// the specific type of data source for routing, processing, or display purposes.
    /// </para>
    /// <para>
    /// The value should align with the actual implementation and data type:
    /// <list type="table">
    /// <listheader>
    /// <term>DataSourceName</term>
    /// <description>Typical Use Case</description>
    /// </listheader>
    /// <item>
    /// <term>QueryableSource</term>
    /// <description>LINQ providers, Entity Framework queries</description>
    /// </item>
    /// <item>
    /// <term>EnumerableSource</term>
    /// <description>In-memory collections, arrays, lists</description>
    /// </item>
    /// <item>
    /// <term>DbContextSource</term>
    /// <description>Entity Framework DbContext operations</description>
    /// </item>
    /// <item>
    /// <term>JsonSource</term>
    /// <description>JSON files, API responses, serialized data</description>
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    public abstract DataSourceName DataSourceName { get; set; }

    /// <summary>
    /// Gets or sets the collection of .NET types that this data source can handle or process.
    /// </summary>
    /// <value>
    /// An enumerable collection of Type objects representing supported data types.
    /// Defaults to an empty collection.
    /// </value>
    /// <remarks>
    /// <para>
    /// This virtual property can be overridden in derived classes to specify which
    /// types the data source supports. The default implementation returns an empty collection.
    /// </para>
    /// <para>
    /// This property can be used for validation, serialization decisions, or UI filtering
    /// to determine compatibility between data sources and target operations.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // For a database source
    /// public override IEnumerable&lt;Type&gt; SupportedTypes { get; set; } = 
    ///     new[] { typeof(string), typeof(int), typeof(DateTime), typeof(decimal) };
    /// 
    /// // For a JSON source
    /// public override IEnumerable&lt;Type&gt; SupportedTypes { get; set; } = 
    ///     new[] { typeof(object), typeof(Dictionary&lt;string, object&gt;) };
    /// </code>
    /// </example>
    public IEnumerable<Type> SupportedTypes { get; set; } = [];

    
    /// <summary>
    /// Creates and returns an appropriate data source adapter based on the current data source configuration.
    /// </summary>
    /// <returns>
    /// An <see cref="IDataSourceAdapter"/> implementation that provides standardized access
    /// to the underlying data source.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method implements the Builder pattern, allowing each derived class to create
    /// the most appropriate adapter for its specific data source type. The adapter provides
    /// a consistent interface for data operations regardless of the underlying data source.
    /// </para>
    /// <para>
    /// The implementation should consider the <see cref="DataSourceName"/> and <see cref="Data"/>
    /// properties to determine the correct adapter type and configuration.
    /// </para>
    /// <para>
    /// This method must be implemented by all derived classes to provide the specific
    /// adapter creation logic for their data source type.
    /// </para>
    /// </remarks>
    /// <exception cref="System.InvalidOperationException">
    /// Thrown when the data source is not properly configured or the Data property
    /// contains invalid data for the specified DataSourceName.
    /// </exception>
    /// <exception cref="System.NotSupportedException">
    /// Thrown when the current configuration is not supported by the adapter implementation.
    /// </exception>
    /// <example>
    /// <code>
    /// public class JsonDataSource : DataSource
    /// {
    ///     public override string FriendlyName { get; init; } = "JSON File";
    ///     public override required object Data { get; init; }
    ///     
    ///     public override IDataSourceAdapter Build()
    ///     {
    ///         return DataSourceName switch
    ///         {
    ///             DataSourceName.JsonSource when Data is string filePath => 
    ///                 new JsonFileAdapter(filePath),
    ///             DataSourceName.JsonSource when Data is JObject jsonObj => 
    ///                 new JsonObjectAdapter(jsonObj),
    ///             _ => throw new InvalidOperationException("Invalid configuration for JSON data source")
    ///         };
    ///     }
    /// }
    /// </code>
    /// </example>
    public abstract IDataSourceAdapter Build<T>();
}