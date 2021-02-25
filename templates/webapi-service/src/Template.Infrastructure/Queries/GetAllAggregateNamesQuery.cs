namespace Template.Infrastructure.Queries
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Dapper;
    using MySqlConnector;
    using Template.Contracts.Responses;
    using Template.Infrastructure.Database;

    public class GetAllAggregateNamesQuery : IGetAllAggregateNamesQuery
    {
        private readonly IConnectionStringProvider connectionStringProvider;
        private readonly SqlHelper<GetAllAggregateNamesQuery> sqlHelper;

        public GetAllAggregateNamesQuery(IConnectionStringProvider connectionStringProvider)
        {
            this.connectionStringProvider = connectionStringProvider;
            this.sqlHelper = new SqlHelper<GetAllAggregateNamesQuery>();
        }

        public async Task<IEnumerable<AggregateName>> ExecuteAsync()
        {
            await using var connection = new MySqlConnection(this.connectionStringProvider.TemplateConnectionString);
            return await connection.QueryAsync<AggregateName>(
                this.sqlHelper.GetSqlFromEmbeddedResource("GetAllDeliveries"),
                commandType: CommandType.Text);
        }
    }
}