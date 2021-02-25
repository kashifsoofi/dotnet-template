namespace Template.Domain.Aggregates.AggregateName
{
    using System;
    using System.Threading.Tasks;

    public interface IAggregateNameAggregateReadRepository
    {
        Task<IAggregateNameAggregate> GetByIdAsync(Guid id);
    }
}
