using System;
namespace Template.Domain.AggregateName
{
    public interface IAggregateNameAggregateFactory
    {
        AggregateNameAggregate Create(Guid id);

        AggregateNameAggregate Create(AggregateNameState state);
    }
}
