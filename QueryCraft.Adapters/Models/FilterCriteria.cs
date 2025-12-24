using QueryCraft.Adapters.Common.Enums;

namespace QueryCraft.Adapters.Models;

/// <summary>
/// Defines the specific criteria for filtering data, including field specifications,
/// comparison values, operators, and options.
/// </summary>
/// <typeparam name="T">The type of values used for comparison in the filter criteria.</typeparam>
/// <remarks>
/// <para>
/// This class encapsulates all the information needed to perform a single filter operation,
/// including the target field, comparison values, operator type, and case sensitivity settings.
/// </para>
/// <para>
/// The generic type parameter <typeparamref name="T"/> represents the type of values
/// being compared, providing type safety for filter operations while maintaining flexibility
/// for different data types.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Filter for users with specific names (case-insensitive)
/// var nameFilter = new FilterCriteria&lt;string&gt;(
///     fieldValues: new[] { "John", "Jane", "Bob" },
///     @operator: FilterOperator.IN,
///     fieldName: "FirstName",
///     caseSensitive: false
/// );
/// 
/// // Filter for users older than 25
/// var ageFilter = new FilterCriteria&lt;int&gt;(
///     fieldValues: new[] { 25 },
///     @operator: FilterOperator.GREATER_THAN,
///     fieldName: "Age"
/// );
/// 
/// // Default filter (empty)
/// var defaultFilter = new FilterCriteria&lt;object&gt;();
/// </code>
/// </example>
public class FilterCriteria<T>
{
    /// <summary>
    /// Gets or sets the name of the field or property to apply the filter to.
    /// </summary>
    /// <value>
    /// A string representing the field name, property name, or column name
    /// that will be used for the filtering operation.
    /// </value>
    /// <remarks>
    /// <para>
    /// This should correspond to a valid field or property name in the target data structure.
    /// The exact format may depend on the underlying data source (e.g., property names for
    /// objects, column names for databases, JSON property names for JSON data).
    /// </para>
    /// <para>
    /// Case sensitivity of field names depends on the underlying data source and
    /// adapter implementation.
    /// </para>
    /// </remarks>
    public string FieldName { get; set; }

    /// <summary>
    /// Gets or sets the collection of values to compare against the specified field.
    /// </summary>
    /// <value>
    /// An enumerable collection of values of type <typeparamref name="T"/> that will be
    /// used for comparison operations based on the specified <see cref="Operator"/>.
    /// </value>
    /// <remarks>
    /// <para>
    /// The number and meaning of values depends on the operator:
    /// <list type="bullet">
    /// <item><description>Single value: EQUALS, NOT_EQUALS, GREATER_THAN, LESS_THAN, etc.</description></item>
    /// <item><description>Multiple values: IN, NOT_IN</description></item>
    /// <item><description>Two values: BETWEEN (min and max values)</description></item>
    /// <item><description>No values: IS_NULL, IS_NOT_NULL</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public IEnumerable<T> FieldValues { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether string comparisons should be case-sensitive.
    /// </summary>
    /// <value>
    /// <c>true</c> if string comparisons should be case-sensitive; otherwise, <c>false</c>.
    /// Defaults to <c>false</c> (case-insensitive).
    /// </value>
    /// <remarks>
    /// <para>
    /// This property only affects string-based comparisons. For non-string types,
    /// this setting is typically ignored by the adapter implementations.
    /// </para>
    /// <para>
    /// The default value of <c>false</c> provides more user-friendly behavior
    /// for most filtering scenarios, but can be overridden when exact case matching is required.
    /// </para>
    /// </remarks>
    public bool CaseSensitive { get; set; } = false;

    /// <summary>
    /// Gets or sets the comparison operator to use for the filtering operation.
    /// </summary>
    /// <value>
    /// A <see cref="FilterOperator"/> value that determines how the <see cref="FieldValues"/>
    /// are compared against the target field values.
    /// </value>
    /// <remarks>
    /// The operator defines the type of comparison to perform, such as equality checks,
    /// range comparisons, pattern matching, or null checks. The choice of operator
    /// affects how the <see cref="FieldValues"/> collection is interpreted and used.
    /// </remarks>
    public FilterOperator Operator { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FilterCriteria{T}"/> class with specified parameters.
    /// </summary>
    /// <param name="fieldValues">The collection of values to use for comparison.</param>
    /// <param name="operator">The comparison operator to apply.</param>
    /// <param name="fieldName">The name of the field to filter on.</param>
    /// <param name="caseSensitive">
    /// Whether string comparisons should be case-sensitive. Defaults to <c>false</c>.
    /// </param>
    /// <remarks>
    /// This constructor allows for full specification of all filter criteria properties,
    /// providing complete control over the filtering behavior.
    /// </remarks>
    /// <example>
    /// <code>
    /// var criteria = new FilterCriteria&lt;string&gt;(
    ///     fieldValues: new[] { "Active", "Pending" },
    ///     @operator: FilterOperator.IN,
    ///     fieldName: "Status",
    ///     caseSensitive: true
    /// );
    /// </code>
    /// </example>
    public FilterCriteria(
        IEnumerable<T> fieldValues, 
        FilterOperator @operator, 
        string fieldName, 
        bool caseSensitive = false)
    {
        Operator = @operator;
        FieldName = fieldName;
        CaseSensitive = caseSensitive;
        FieldValues = fieldValues;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FilterCriteria{T}"/> class with default values.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This parameterless constructor creates a filter criteria with default values:
    /// <list type="bullet">
    /// <item><description>FieldValues: Empty collection</description></item>
    /// <item><description>Operator: EQUALS</description></item>
    /// <item><description>FieldName: Empty string</description></item>
    /// <item><description>CaseSensitive: false</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// This constructor is useful for object initialization scenarios or when
    /// creating placeholder filter criteria that will be configured later.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var criteria = new FilterCriteria&lt;string&gt;
    /// {
    ///     FieldName = "Name",
    ///     FieldValues = new[] { "John" },
    ///     Operator = FilterOperator.CONTAINS
    /// };
    /// </code>
    /// </example>
    public FilterCriteria(): this(
        [], 
        FilterOperator.EQUALS, 
        string.Empty)
    {
        
    }
}