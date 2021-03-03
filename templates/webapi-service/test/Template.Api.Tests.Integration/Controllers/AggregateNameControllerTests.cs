namespace Template.Api.Tests.Integration.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Newtonsoft.Json;
    using Template.Contracts.Responses;
    using Xunit;

    public class AggregateNameControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public AggregateNameControllerTests(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task Get_should_return_ok_with_AggregateName()
        {
            // Arrange
            var id = Guid.NewGuid();
            var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/aggregatename/{id}");

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync();
            var aggregatename = JsonConvert.DeserializeObject<AggregateName>(responseContent);
            aggregatename.Id.Should().Be(id);
        }
    }
}
