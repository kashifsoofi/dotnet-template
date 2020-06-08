namespace Template.Domain.Aggregates.AggregateName
{
    using System;

    public class AggregateNameAggregateFactory : IAggregateNameAggregateFactory
    {
        public AggregateNameAggregate Create(Guid id)
        {
            return new AggregateNameAggregate(id);
        }

        public AggregateNameAggregate Create(AggregateNameAggregateState state)
        {
            return new AggregateNameAggregate(state);
        }
    }
}
