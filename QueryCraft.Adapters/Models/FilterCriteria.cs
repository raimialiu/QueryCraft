using QueryCraft.Adapters.Common.Enums;

namespace QueryCraft.Adapters.Models;

public class FilterCriteria<T>
{
    public string FieldName { get; set; }
    public IEnumerable<T> FieldValues { get; set; }
    public bool CaseSensitive { get; set; } = false;
    public FilterOperator Operator { get; set; }

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

    public FilterCriteria(): this(
        [], 
        FilterOperator.EQUALS, 
        string.Empty)
    {
        
    }
}