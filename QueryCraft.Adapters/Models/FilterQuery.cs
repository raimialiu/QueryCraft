using QueryCraft.Adapters.Common.Enums;

namespace QueryCraft.Adapters.Models;

/// <summary>
/// Represents an individual filter query with its associated condition and optional column specifications.
/// </summary>
/// <typeparam name="T">The type of data that the filter query operates on.</typeparam>
/// <remarks>
/// <para>
/// A filter query encapsulates a single filtering operation, including the criteria to apply,
/// the logical condition for combining with other queries, and optional column specifications
/// for more granular control over the filtering process.
/// </para>
/// <para>
/// This class serves as a bridge between high-level filter specifications and the underlying
/// filter criteria, providing additional metadata for query execution.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var filterQuery = new FilterQuery&lt;User&gt;
/// {
///     Query = new FilterCriteria&lt;string&gt;(
///         fieldValues: new[] { "Admin", "Manager" },
///         @operator: FilterOperator.IN,
///         fieldName: "Role",
///         caseSensitive: false
///     ),
///     Condition = FilterCondition.And,
///     Columns = new[]
///     {
///         new FilterColumn { Name = "Role", DataType = typeof(string) }
///     }
/// };
/// </code>
/// </example>
public class FilterQuery<T>
{
    /// <summary>
    /// Gets or sets the filter criteria that defines the specific filtering logic for this query.
    /// </summary>
    /// <value>
    /// A <see cref="FilterCriteria{T}"/> object that contains the field name, values,
    /// operator, and other settings for the filter operation.
    /// </value>
    /// <remarks>
    /// This property contains the core filtering logic, including what field to filter on,
    /// what values to compare against, and what comparison operator to use.
    /// The value is immutable after object construction due to the init accessor.
    /// </remarks>
    public FilterCriteria<T> Query { get; init; }

    /// <summary>
    /// Gets or sets the logical condition that determines how this query combines with adjacent queries.
    /// </summary>
    /// <value>
    /// A <see cref="FilterCondition"/> value that specifies the logical operator
    /// (e.g., AND, OR) for combining this query with others.
    /// </value>
    /// <remarks>
    /// <para>
    /// This condition is used when multiple queries exist within the same filter group
    /// or when combining results from different queries. It determines the logical
    /// relationship between this query and its neighbors.
    /// </para>
    /// <para>
    /// The value is immutable after object construction due to the init accessor.
    /// </para>
    /// </remarks>
    public FilterCondition Condition { get; init; }

    /// <summary>
    /// Gets or sets the optional collection of column specifications that provide additional
    /// metadata for the filtering operation.
    /// </summary>
    /// <value>
    /// An enumerable collection of <see cref="FilterColumn"/> objects that describe
    /// the columns involved in the filter operation. Defaults to an empty collection.
    /// </value>
    /// <remarks>
    /// <para>
    /// This property provides additional metadata about the columns being filtered,
    /// which can be useful for query optimization, validation, or UI generation.
    /// It is optional and defaults to an empty collection when not specified.
    /// </para>
    /// <para>
    /// Column specifications can include information such as data types, display names,
    /// formatting options, or other metadata relevant to the filtering process.
    /// </para>
    /// </remarks>
    public IEnumerable<FilterColumn>? Columns { get; set; } = [];
}

// Select FirstName as Name where
//
// ( (x.FirstName = 'Tunde' and x.LastName = 'Kehinde') OR
// x.Username = 'Tunde' and x.Email = 'tunde@gmail.com))
// And 
// (x.Phone = 2)

//
/*
var filter = new FilterCriteria<string>(
        ["Tunde"],
            FilterOperator.EQUALS,
            "FirstName"
    );
    
    var filter2 = new FilterCriteria<string>(
        ["Kehinde"],
        FilterOperator.EQUALS,
        "LastName"
    );
    */