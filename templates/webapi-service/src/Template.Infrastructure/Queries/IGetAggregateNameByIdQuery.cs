namespace Template.Infrastructure.Queries
{
    using System;
    using System.Threading.Tasks;
    using Template.Contracts.Responses;

    public interface IGetAggregateNameByIdQuery
    {
        Task<AggregateName> ExecuteAsync(Guid id);
    }
}
