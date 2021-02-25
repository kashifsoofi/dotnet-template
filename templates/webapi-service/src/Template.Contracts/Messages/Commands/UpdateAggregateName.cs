namespace Template.Contracts.Messages.Commands
{
    using System;

    public class UpdateAggregateName
    {
        public Guid Id { get; set; }

        public UpdateAggregateName(Guid id)
        {
            Id = id;
        }
    }
}