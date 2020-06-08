namespace Template.Infrastructure.Tests.Integration.AggregateName
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Template.Contracts.Responses;
    using Template.Infrastructure.AggregateName;
    using Template.Infrastructure.Tests.Integration.DockerClient;
    using Template.Infrastructure.Tests.Integration.Testcontainers;
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
