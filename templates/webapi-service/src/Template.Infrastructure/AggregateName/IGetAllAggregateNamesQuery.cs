namespace Template.Infrastructure.AggregateName
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Template.Contracts.Responses;

    public interface IGetAllAggregateNamesQuery
    {
        Task<List<AggregateName>> ExecuteAsync();
    }
}
