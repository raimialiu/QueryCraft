namespace QueryCraft.Adapters.Common.Enums;

/// <summary>
/// Defines the available types of data sources that can be used in the system.
/// </summary>
/// <remarks>
/// This enumeration categorizes different data source implementations based on
/// their underlying data access patterns and capabilities.
/// </remarks>
public enum DataSourceName
{
    /// <summary>
    /// Represents a data source that implements IQueryable&lt;T&gt; interface,
    /// allowing for deferred execution and expression tree-based querying.
    /// </summary>
    /// <remarks>
    /// Typically used for LINQ-to-SQL, Entity Framework queries, or other
    /// providers that support expression tree translation.
    /// </remarks>
    QueryableSource,

    /// <summary>
    /// Represents a data source that implements IEnumerable&lt;T&gt; interface,
    /// providing in-memory collection-based data access.
    /// </summary>
    /// <remarks>
    /// Used for collections, arrays, lists, or other in-memory data structures
    /// that support enumeration but not queryable expression trees.
    /// </remarks>
    EnumerableSource,

    /// <summary>
    /// Represents a data source that uses Entity Framework DbContext
    /// for database operations and entity management.
    /// </summary>
    /// <remarks>
    /// Specifically designed for Entity Framework-based data access,
    /// providing change tracking, lazy loading, and ORM capabilities.
    /// </remarks>
    DbContextSource,

    /// <summary>
    /// Represents a data source that handles JSON-formatted data,
    /// either from files, web APIs, or serialized objects.
    /// </summary>
    /// <remarks>
    /// Used for JSON files, REST API responses, or any JSON-serialized
    /// data that needs to be parsed and processed.
    /// </remarks>
    JsonSource
}