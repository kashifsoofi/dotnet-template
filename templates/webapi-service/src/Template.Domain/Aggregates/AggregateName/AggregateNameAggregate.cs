namespace Template.Domain.Aggregates.AggregateName
{
    using System;
    using System.Collections.Generic;
    using Template.Contracts.Messages.Commands;
    using Template.Contracts.Messages.Events;

    public class AggregateNameAggregate : IAggregateNameAggregate
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

        public bool IsNew => isNew;

        public void Create(CreateAggregateName command)
        {
            throw new NotImplementedException();
        }

        public void Update(UpdateAggregateName command)
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public List<IAggregateEvent> UncommittedEvents { get; set; } = new List<IAggregateEvent> { };
    }
}
