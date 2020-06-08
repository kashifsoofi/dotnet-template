namespace Template.Domain.Aggregates.AggregateName
{
    using System;

    public class AggregateNameAggregateState : IAggregateNameAggregateState
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
