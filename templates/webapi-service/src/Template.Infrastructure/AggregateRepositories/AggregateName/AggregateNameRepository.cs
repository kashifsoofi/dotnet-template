namespace Template.Infrastructure.AggregateRepositories.AggregateName
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using Dapper;
    using MySqlConnector;
    using Template.Domain.Aggregates.AggregateName;
    using Template.Infrastructure.Database;

    public class AggregateNameRepository : IAggregateNameAggregateReadRepository, IAggregateNameAggregateWriteRepository
    {
        private readonly IConnectionStringProvider connectionStringProvider;
        private readonly IAggregateNameAggregateFactory aggregateFactory;
        private readonly SqlHelper<AggregateNameRepository> sqlHelper;

        public AggregateNameRepository(IConnectionStringProvider connectionStringProvider, IAggregateNameAggregateFactory aggregateFactory)
        {
            this.connectionStringProvider = connectionStringProvider;
            this.aggregateFactory = aggregateFactory;
            this.sqlHelper = new SqlHelper<AggregateNameRepository>();
        }

        public async Task<IAggregateNameAggregate> GetByIdAsync(Guid id)
        {
            await using var connection = new MySqlConnection(this.connectionStringProvider.TemplateConnectionString);
            {
                var state = await connection.QueryFirstOrDefaultAsync<AggregateNameAggregateState>(
                    this.sqlHelper.GetSqlFromEmbeddedResource("GetAggregateNameById"),
                    new { Id = id },
                    commandType: CommandType.Text);

                return state == null ? aggregateFactory.Create(id) : aggregateFactory.Create(state);
            }
        }

        public async Task CreateAsync(IAggregateNameAggregate aggregate)
        {
            await using var connection = new MySqlConnection(this.connectionStringProvider.TemplateConnectionString);
            {
                var parameters = new
                {
                    aggregate.Id,
                    aggregate.State.CreatedOn,
                    aggregate.State.UpdatedOn,
                };

                await connection.ExecuteAsync(this.sqlHelper.GetSqlFromEmbeddedResource("CreateAggregateName"), parameters,
                    commandType: CommandType.Text);
            }
        }

        public async Task UpdateAsync(IAggregateNameAggregate aggregate)
        {
            await using var connection = new MySqlConnection(this.connectionStringProvider.TemplateConnectionString);
            {
                var parameters = new
                {
                    aggregate.Id,
                    aggregate.State.UpdatedOn,
                };

                await connection.ExecuteAsync(this.sqlHelper.GetSqlFromEmbeddedResource("UpdateAggregateName"), parameters,
                    commandType: CommandType.Text);
            }
        }

        public async Task DeleteAsync(IAggregateNameAggregate aggregate)
        {
            await using var connection = new MySqlConnection(this.connectionStringProvider.TemplateConnectionString);
            {
                var parameters = new
                {
                    aggregate.Id,
                };

                await connection.ExecuteAsync(this.sqlHelper.GetSqlFromEmbeddedResource("DeleteAggregateName"), parameters,
                    commandType: CommandType.Text);
            }
        }
    }
}
