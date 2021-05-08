namespace Template.Domain.Aggregates.AggregateName
{
    using System.Threading.Tasks;

    public interface IAggregateNameAggregateWriteRepository
    {
        Task CreateAsync(IAggregateNameAggregate aggregate);

        Task UpdateAsync(IAggregateNameAggregate aggregate);

        Task DeleteAsync(IAggregateNameAggregate aggregate);
    }
}
