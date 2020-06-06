namespace Template.Infrastructure.AggregateName
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using MySql.Data.MySqlClient;
    using Template.Infrastructure.Database;
    using Template.Contracts.Responses;
    using Dapper;

    public class GetAllAggregateNamesQuery : IGetAllAggregateNamesQuery
    {
        private readonly IConnectionStringProvider connectionStringProvider;

        public GetAllAggregateNamesQuery(IConnectionStringProvider connectionStringProvider)
        {
            this.connectionStringProvider = connectionStringProvider;
        }

        public async Task<List<AggregateName>> ExecuteAsync()
        {
            var sql = "SELECT Id, CreatedOn, UpdatedOn FROM AggregateName ORDER BY CreatedOn DESC";

            using (var connection = new MySqlConnection(this.connectionStringProvider.TemplateConnectionString))
            {
                var query = await connection.QueryAsync<AggregateName>(sql);
                return query.ToList();
            }
        }
    }
}
