namespace Template.Infrastructure.Tests.Integration.Queries
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Template.Infrastructure.Queries;
    using Template.Infrastructure.Tests.Integration.DockerClient;
    using Xunit;

    [Collection("Database collection")]
    public class GetAllAggregateNamesQueryTests
    {
        private readonly GetAllAggregateNamesQuery sut;

        public GetAllAggregateNamesQueryTests(DockerFixture fixture)
        {
            this.sut = new GetAllAggregateNamesQuery(fixture.ConnectionStringProvider);
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
