using QueryCraft.Adapters.Common.Enums;

namespace QueryCraft.Adapters.Models;

public class FilterGroup<T>
{
    public List<FilterQuery<T>> Queries { get; set; }
    public FilterCondition Condition { get; init; }
}