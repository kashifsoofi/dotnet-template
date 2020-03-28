using System;
namespace Template.Domain.AggregateName
{
    public class AggregateNameAggregateFactory : IAggregateNameAggregateFactory
    {
        public AggregateNameAggregate Create(Guid id)
        {
            throw new NotImplementedException();
        }

        public AggregateNameAggregate Create(AggregateNameState state)
        {
            throw new NotImplementedException();
        }
    }
}
