using QueryCraft.Adapters.Common.Enums;

namespace QueryCraft.Adapters.Models;

/// <summary>
/// Represents a logical grouping of filter queries that are combined using a specified condition,
/// with optional pagination support.
/// </summary>
/// <typeparam name="T">The type of data that the filter group operates on.</typeparam>
/// <remarks>
/// <para>
/// A filter group allows multiple filter queries to be logically combined using AND/OR operations.
/// This enables complex filtering scenarios where multiple criteria need to be applied together
/// with specific logical relationships.
/// </para>
/// <para>
/// The group's condition determines how individual queries within the group are combined,
/// providing flexibility for creating sophisticated filter expressions.
/// </para>
/// <para>
/// Optional pagination support allows for efficient handling of large result sets by
/// specifying page size and offset parameters.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var filterGroup = new FilterGroup&lt;User&gt;
/// {
///     Condition = FilterCondition.And,
///     Queries = new List&lt;FilterQuery&lt;User&gt;&gt;
///     {
///         new FilterQuery&lt;User&gt;
///         {
///             Query = new FilterCriteria&lt;string&gt;(new[] { "John", "Jane" }, FilterOperator.IN, "FirstName"),
///             Condition = FilterCondition.And
///         },
///         new FilterQuery&lt;User&gt;
///         {
///             Query = new FilterCriteria&lt;int&gt;(new[] { 18 }, FilterOperator.GREATER_THAN, "Age"),
///             Condition = FilterCondition.And
///         }
///     },
///     PaginationQuery = new PaginationQuery { PageSize = 10, PageNumber = 1 }
/// };
/// </code>
/// </example>
public class FilterGroup<T>
{
    /// <summary>
    /// Gets or sets the collection of filter queries that belong to this group.
    /// </summary>
    /// <value>
    /// A list of <see cref="FilterQuery{T}"/> objects that define the individual filter criteria
    /// to be applied within this group.
    /// </value>
    /// <remarks>
    /// Each query in the collection represents a specific filtering condition that will be
    /// combined with other queries according to the group's <see cref="Condition"/> property.
    /// The order of queries may affect performance but should not affect the logical result.
    /// </remarks>
    public List<FilterQuery<T>> Queries { get; set; } = [];

    /// <summary>
    /// Gets or sets the logical condition used to combine all queries within this filter group.
    /// </summary>
    /// <value>
    /// A <see cref="FilterCondition"/> value that determines how the queries in this group
    /// are logically combined (e.g., AND, OR).
    /// </value>
    /// <remarks>
    /// <para>
    /// This condition applies to all queries within the <see cref="Queries"/> collection,
    /// creating expressions like: (Query1 AND Query2 AND Query3) or (Query1 OR Query2 OR Query3).
    /// </para>
    /// <para>
    /// The value is immutable after object construction due to the init accessor.
    /// </para>
    /// </remarks>
    public FilterCondition Condition { get; init; }
    
    /// <summary>
    /// Gets or sets the optional pagination configuration for limiting and offsetting results.
    /// </summary>
    /// <value>
    /// A <see cref="PaginationQuery"/> object that specifies page size, page number, or offset
    /// parameters for result pagination. Can be null if pagination is not required.
    /// </value>
    /// <remarks>
    /// <para>
    /// When specified, pagination is applied after all filtering operations are completed.
    /// This allows for efficient handling of large result sets by retrieving only the
    /// requested subset of data.
    /// </para>
    /// <para>
    /// The pagination behavior depends on the underlying adapter implementation and
    /// data source capabilities. Some adapters may perform pagination at the database level
    /// for optimal performance, while others may apply it in memory.
    /// </para>
    /// <para>
    /// The value is immutable after object construction due to the init accessor.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Pagination with page-based approach
    /// PaginationQuery = new PaginationQuery 
    /// { 
    ///     PageSize = 25, 
    ///     PageNumber = 2 
    /// };
    /// 
    /// // Pagination with offset-based approach
    /// PaginationQuery = new PaginationQuery 
    /// { 
    ///     PageSize = 50, 
    ///     Offset = 100 
    /// };
    /// </code>
    /// </example>
    public PaginationQuery? PaginationQuery { get; init; }
}