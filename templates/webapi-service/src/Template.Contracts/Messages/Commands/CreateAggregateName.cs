namespace Template.Contracts.Messages.Commands
{
    using System;

    public class CreateAggregateName
    {
        public Guid Id { get; set; }

        public CreateAggregateName(Guid id)
        {
            Id = id;
        }
    }
}
