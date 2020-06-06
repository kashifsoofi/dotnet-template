namespace Template.Contracts.Messages.Events
{
    using System;

    public interface IAggregateNameCreated
    {
        Guid Id { get; }
        DateTime CreatedOn { get; }
    }
}
