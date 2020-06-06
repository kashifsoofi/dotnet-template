namespace Template.Contracts.Messages.Commands
{
    using System;

    public class CreateAggregateName
    {
        public int Id { get; }
        public DateTime CreatedOn { get; }
        public DateTime UpdatedOn { get; }

        public CreateAggregateName(int id)
        {
            Id = id;
        }
    }
}
