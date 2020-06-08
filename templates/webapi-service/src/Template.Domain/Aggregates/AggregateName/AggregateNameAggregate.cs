namespace Template.Domain.Aggregates.AggregateName
{
    using System;
    using Template.Contracts.Messages.Commands;

    public class AggregateNameAggregate
    {
        private readonly AggregateNameAggregateState state;
        private readonly bool isNew;

        public AggregateNameAggregate(Guid id)
        {
            this.state = new AggregateNameAggregateState { Id = id };
            this.isNew = true;
        }

        public AggregateNameAggregate(AggregateNameAggregateState state)
        {
            this.state = state ?? throw new ArgumentNullException(nameof(state));
            this.isNew = false;
        }

        public Guid Id => this.state.Id;

        public IAggregateNameAggregateState State => this.state;
    }
}
