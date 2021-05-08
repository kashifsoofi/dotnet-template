namespace Template.Infrastructure.Tests.Integration.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoFixture.Xunit2;
    using FluentAssertions;
    using Template.Domain.Aggregates.AggregateName;
    using Template.Infrastructure.Queries;
    using Template.Infrastructure.Tests.Integration.Helpers;
    using Xunit;

    [Collection("DatabaseCollection")]
    public class GetAllAggregateNamesQueryTests : IAsyncLifetime
    {
        private readonly DatabaseHelper<Guid, AggregateNameAggregateState> aggregateNameDatabaseHelper;

        private readonly GetAllAggregateNamesQuery sut;

        public GetAllAggregateNamesQueryTests(DatabaseFixture databaseFixture)
        {
            var connectionStringProvider = databaseFixture.ConnectionStringProvider;

            aggregateNameDatabaseHelper = new DatabaseHelper<Guid, AggregateNameAggregateState>("AggregateName", connectionStringProvider.TemplateConnectionString, x => x.Id);

            this.sut = new GetAllAggregateNamesQuery(connectionStringProvider);
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            await aggregateNameDatabaseHelper.CleanTableAsync();
        }

        [Fact(Skip = "TestContainers need updating")]
        public async Task ExecuteAsync_GivenNoRecords_ShouldReturnEmptyCollection()
        {
            // Arrange
            var result = await this.sut.ExecuteAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [Theory(Skip = "TestContainers need updating")]
        [AutoData]
        public async Task ExecuteAsync_GivenRecordsExist_ShouldReturnRecords(List<AggregateNameAggregateState> states)
        {
            // Arrange
            await this.aggregateNameDatabaseHelper.AddRecordsAsync(states);

            // Act
            var result = await this.sut.ExecuteAsync();

            // Assert
            result.Should().BeEquivalentTo(states);
        }
    }
}
