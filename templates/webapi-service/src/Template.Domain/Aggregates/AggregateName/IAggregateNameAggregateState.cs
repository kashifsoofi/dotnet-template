namespace Template.Domain.Aggregates.AggregateName
{
    using System;

    public interface IAggregateNameAggregateState
    {
        Guid Id { get; }
        DateTime CreatedOn { get; }
        DateTime UpdatedOn { get; }
    }
}
