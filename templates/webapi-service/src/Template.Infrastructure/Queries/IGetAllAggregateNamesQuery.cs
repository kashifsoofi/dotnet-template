namespace Template.Infrastructure.Queries
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Template.Contracts.Responses;

    public interface IGetAllAggregateNamesQuery
    {
        Task<IEnumerable<AggregateName>> ExecuteAsync();
    }
}
