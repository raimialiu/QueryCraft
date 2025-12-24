namespace QueryCraft.Adapters.Models;

/// <summary>
/// Represents the complete result of a query operation, including both the data results
/// and associated metadata about the query execution.
/// </summary>
/// <typeparam name="T">The type of objects contained in the query result.</typeparam>
/// <remarks>
/// <para>
/// This class encapsulates both the actual query results and metadata that provides
/// additional information about the query execution, such as the generated query string,
/// pagination details, performance metrics, and other diagnostic information.
/// </para>
/// <para>
/// The separation of results and metadata allows for clean data access while providing
/// transparency into the query execution process for debugging, logging, and optimization purposes.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var queryResult = new QueryResult&lt;User&gt;
/// {
///     Metadata = new QueryMetadata
///     {
///         QueryString = "SELECT * FROM Users WHERE Age > 18 AND IsActive = 1",
///         PaginationQuery = new PaginationQuery { PageSize = 10, PageNumber = 1 }
///     },
///     Result = new PaginatedResult&lt;User&gt;
///     {
///         Data = users,
///         TotalCount = 150,
///         PageSize = 10,
///         CurrentPage = 1
///     }
/// };
/// </code>
/// </example>
public class QueryResult<T>
{
    /// <summary>
    /// Gets or sets the metadata associated with the query execution.
    /// </summary>
    /// <value>
    /// A <see cref="QueryMetadata"/> object containing information about the query execution,
    /// such as the generated query string and pagination parameters.
    /// </value>
    /// <remarks>
    /// <para>
    /// The metadata provides transparency into how the query was executed, which is valuable
    /// for debugging, performance analysis, logging, and audit purposes.
    /// </para>
    /// <para>
    /// The value is immutable after object construction due to the init accessor.
    /// </para>
    /// </remarks>
    public QueryMetadata Metadata { get; init; }

    /// <summary>
    /// Gets or sets the paginated result data from the query execution.
    /// </summary>
    /// <value>
    /// A <see cref="PaginatedResult{T}"/> object containing the actual data results
    /// along with pagination information such as total count and page details.
    /// </value>
    /// <remarks>
    /// <para>
    /// This property contains both the filtered data and pagination metadata,
    /// providing a complete picture of the result set including information about
    /// the total number of matching records and the current page position.
    /// </para>
    /// <para>
    /// The value is immutable after object construction due to the init accessor.
    /// </para>
    /// </remarks>
    public PaginatedResult<T> Result { get; init; }
}