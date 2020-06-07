using System.Threading.Tasks;
using FluentAssertions;
using Template.Infrastructure.AggregateName;
using Xunit;

namespace Template.Infrastructure.Tests.Integration.AggregateName
{
    [Collection("Database collection")]
    public class GetAllAggregateNamesQueryTests
    {
        private readonly DatabaseFixture databaseFixture;

        private readonly GetAllAggregateNamesQuery sut;

        public GetAllAggregateNamesQueryTests(DatabaseFixture databaseFixture)
        {
            this.databaseFixture = databaseFixture;

            this.sut = new GetAllAggregateNamesQuery(this.databaseFixture.ConnectionStringProvider);
        }

        [Fact]
        public async Task ExecuteAsync_GivenNoRecords_ShouldReturnEmptyCollection()
        {
            // A
            var aggregatenameList = await this.sut.ExecuteAsync();

            // Assert
            aggregatenameList.Should().BeEmpty();
        }
    }
}
