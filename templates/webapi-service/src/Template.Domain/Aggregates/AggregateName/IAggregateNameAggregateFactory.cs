namespace Template.Domain.Aggregates.AggregateName
{
    using System;

    public interface IAggregateNameAggregateFactory
    {
        AggregateNameAggregate Create(Guid id);

        AggregateNameAggregate Create(AggregateNameAggregateState state);
    }
}
