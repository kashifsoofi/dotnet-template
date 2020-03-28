using System;
namespace Template.Domain.AggregateName
{
    public class AggregateNameAggregate
    {
        public AggregateNameAggregate(IAggregateNameState state)
        {
            this.State = state;
        }

        public IAggregateNameState State { get; }
    }
}
