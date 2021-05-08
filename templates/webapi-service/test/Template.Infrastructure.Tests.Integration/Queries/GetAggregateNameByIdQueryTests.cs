namespace Template.Infrastructure.Tests.Integration.Queries
{
    using System;
    using System.Threading.Tasks;
    using AutoFixture.Xunit2;
    using FluentAssertions;
    using Template.Domain.Aggregates.AggregateName;
    using Template.Infrastructure.Queries;
    using Template.Infrastructure.Tests.Integration.Helpers;
    using Xunit;

    [Collection("DatabaseCollection")]
    public class GetAggregateNameByIdQueryTests : IAsyncLifetime
    {
        private readonly DatabaseHelper<Guid, AggregateNameAggregateState> aggregateNameDatabaseHelper;

        private readonly GetAggregateNameByIdQuery sut;

        public GetAggregateNameByIdQueryTests(DatabaseFixture databaseFixture)
        {
            var connectionStringProvider = databaseFixture.ConnectionStringProvider;

            aggregateNameDatabaseHelper = new DatabaseHelper<Guid, AggregateNameAggregateState>("AggregateName", connectionStringProvider.TemplateConnectionString, x => x.Id);

            this.sut = new GetAggregateNameByIdQuery(connectionStringProvider);
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            await aggregateNameDatabaseHelper.CleanTableAsync();
        }

        [Theory(Skip = "TestContainers need updating")]
        [AutoData]
        public async Task ExecuteAsync_GivenNoRecordExists_ShouldReturnNull(Guid id)
        {
            // Arrange
            var result = await this.sut.ExecuteAsync(id);

            // Assert
            result.Should().BeNull();
        }

        [Theory(Skip = "TestContainers need updating")]
        [AutoData]
        public async Task ExecuteAsync_GivenRecordExists_ShouldReturnAggregateName(AggregateNameAggregateState state)
        {
            // Arrange
            await this.aggregateNameDatabaseHelper.AddRecordAsync(state);

            // Act
            var result = await this.sut.ExecuteAsync(state.Id);

            // Assert
            result.Should().BeEquivalentTo(
                state,
                x => x
                    .Excluding(p => p.CreatedOn)
                    .Excluding(p => p.UpdatedOn));
        }
    }
}
