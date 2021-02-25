namespace Template.Domain.Aggregates.AggregateName
{
    using System;
    using System.Collections.Generic;
    using Template.Contracts.Messages.Commands;
    using Template.Contracts.Messages.Events;

    public interface IAggregateNameAggregate
    {
        Guid Id { get; }

        IAggregateNameAggregateState State { get; }

        bool IsNew { get; }

        void Create(CreateAggregateName command);

        void Update(UpdateAggregateName command);

        void Delete();

        List<IAggregateEvent> UncommittedEvents { get; set; }
    }
}