using QueryCraft.Adapters.Common.Enums;

namespace QueryCraft.Adapters.Models;

public class FilterQuery<T>
{
    public FilterCriteria<T> Query { get; init; }
    public FilterCondition Condition { get; init; }
    public IEnumerable<FilterColumn>? Columns { get; set; } = [];
}

// Select FirstName as Name where
//
// ( (x.FirstName = 'Tunde' and x.LastName = 'Kehinde') OR
// x.Username = 'Tunde' and x.Email = 'tunde@gmail.com))
// And 
// (x.Phone = 2)

//
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