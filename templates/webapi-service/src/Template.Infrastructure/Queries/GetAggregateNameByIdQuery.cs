namespace Template.Infrastructure.Queries
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using Dapper;
    using MySqlConnector;
    using Template.Contracts.Responses;
    using Template.Infrastructure.Database;

    public class GetAggregateNameByIdQuery : IGetAggregateNameByIdQuery
    {
        private readonly IConnectionStringProvider connectionStringProvider;
        private readonly SqlHelper<GetAggregateNameByIdQuery> sqlHelper;

        public GetAggregateNameByIdQuery(IConnectionStringProvider connectionStringProvider)
        {
            this.connectionStringProvider = connectionStringProvider;
            this.sqlHelper = new SqlHelper<GetAggregateNameByIdQuery>();
        }

        public async Task<AggregateName> ExecuteAsync(Guid id)
        {
            await using var connection = new MySqlConnection(this.connectionStringProvider.TemplateConnectionString);
            return await connection.QueryFirstOrDefaultAsync<AggregateName>(
                this.sqlHelper.GetSqlFromEmbeddedResource("GetAggregateNameById"),
                new { Id = id },
                commandType: CommandType.Text);
        }
    }
}