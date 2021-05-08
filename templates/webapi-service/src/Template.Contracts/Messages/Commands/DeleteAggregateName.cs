namespace Template.Contracts.Messages.Commands
{
    using System;

    public class DeleteAggregateName
    {
        public Guid Id { get; set; }

        public DeleteAggregateName(Guid id)
        {
            Id = id;
        }
    }
}