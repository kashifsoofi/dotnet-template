namespace Template.Infrastructure.Tests.Integration.AggregateRepositories.AggregateName
{
    using System;
    using System.Threading.Tasks;
    using AutoFixture.Xunit2;
    using FluentAssertions;
    using Template.Domain.Aggregates.AggregateName;
    using Template.Infrastructure.AggregateRepositories.AggregateName;
    using Template.Infrastructure.Database;
    using Template.Infrastructure.Tests.Integration.Helpers;
    using Xunit;

    [Collection("DatabaseCollection")]
    public class AggregateNameRepositoryTests : IAsyncLifetime
    {
        private readonly DatabaseHelper<Guid, AggregateNameAggregateState> aggregateNameDatabaseHelper;

        private readonly AggregateNameRepository sut;

        public AggregateNameRepositoryTests(DatabaseFixture databaseFixture)
        {
            var connectionStringProvider = databaseFixture.ConnectionStringProvider;

            aggregateNameDatabaseHelper = new DatabaseHelper<Guid, AggregateNameAggregateState>("AggregateName", connectionStringProvider.TemplateConnectionString, x => x.Id);

            var factory = new AggregateNameAggregateFactory();
            sut = new AggregateNameRepository(connectionStringProvider, factory);
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            await aggregateNameDatabaseHelper.CleanTableAsync();
        }

        [Theory(Skip = "TestContainers need updating")]
        [AutoData]
        public async void GetByIdAsync_GivenRecordDoesNotExist_ShouldReturnNewAggregate(Guid newId)
        {
            // Arrange
            // Act
            var result = await this.sut.GetByIdAsync(newId);

            // Assert
            result.IsNew.Should().BeTrue();
            result.UncommittedEvents.Should().BeEmpty();
        }

        [Theory(Skip = "TestContainers need updating")]
        [AutoData]
        public async void GetByIdAsync_GivenRecordExists_ShouldReturnAggregate(AggregateNameAggregateState state)
        {
            // Arrange
            await this.aggregateNameDatabaseHelper.AddRecordAsync(state);

            // Act
            var result = await this.sut.GetByIdAsync(state.Id);

            // Assert
            result.IsNew.Should().BeFalse();
            result.UncommittedEvents.Should().BeEmpty();
            result.Id.Should().Be(state.Id);
            result.State.Should().BeEquivalentTo(
                state,
                x => x
                    .Excluding(p => p.CreatedOn)
                    .Excluding(p => p.UpdatedOn));
            result.State.CreatedOn.Should().BeCloseTo(state.CreatedOn, TimeSpan.FromSeconds(1));
            result.State.UpdatedOn.Should().BeCloseTo(state.UpdatedOn, TimeSpan.FromSeconds(1));
        }

        [Theory(Skip = "TestContainers need updating")]
        [AutoData]
        public async void CreateAsync_GivenRecordDoesNotExist_CreatesRecord(AggregateNameAggregateState state)
        {
            // Arrange
            // Act
            await this.sut.CreateAsync(new AggregateNameAggregate(state));
            this.aggregateNameDatabaseHelper.TrackId(state.Id);

            var result = (await this.sut.GetByIdAsync(state.Id)).State;

            // Assert
            result.Id.Should().Be(state.Id);
            result.Should().BeEquivalentTo(
                state,
                x => x
                    .Excluding(p => p.CreatedOn)
                    .Excluding(p => p.UpdatedOn));
            result.CreatedOn.Should().BeCloseTo(state.CreatedOn, TimeSpan.FromSeconds(1));
            result.UpdatedOn.Should().BeCloseTo(state.UpdatedOn, TimeSpan.FromSeconds(1));
        }

        [Theory(Skip = "TestContainers need updating")]
        [AutoData]
        public async void UpdateAsync_GivenRecordExists_UpdatesRecord(AggregateNameAggregateState state, AggregateNameAggregateState updatedState)
        {
            //// non updateable properties
            updatedState.CreatedOn = state.CreatedOn;

            // Arrange
            await this.aggregateNameDatabaseHelper.AddRecordAsync(state);

            // Act
            updatedState.Id = state.Id;
            await this.sut.UpdateAsync(new AggregateNameAggregate(updatedState));

            var result = (await this.sut.GetByIdAsync(state.Id)).State;

            // Assert
            result.Id.Should().Be(state.Id);
            result.Should().BeEquivalentTo(
                updatedState,
                x => x
                    .Excluding(p => p.CreatedOn)
                    .Excluding(p => p.UpdatedOn));
            result.CreatedOn.Should().BeCloseTo(updatedState.CreatedOn, TimeSpan.FromSeconds(1));
            result.UpdatedOn.Should().BeCloseTo(updatedState.UpdatedOn, TimeSpan.FromSeconds(1));
        }
    }
}
