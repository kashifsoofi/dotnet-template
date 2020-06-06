namespace Template.Contracts.Messages.Events
{
    using System;

    public class AggregateNameCreated : IAggregateNameCreated
    {
        public Guid Id { get; }

        public DateTime CreatedOn { get; }

        public AggregateNameCreated(Guid id, DateTime createdOn)
        {
            this.Id = id;
            this.CreatedOn = createdOn;
        }
    }
}
