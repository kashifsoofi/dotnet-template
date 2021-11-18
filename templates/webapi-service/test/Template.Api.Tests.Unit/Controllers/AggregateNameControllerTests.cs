namespace Template.Api.Tests.Unit.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using NServiceBus.Testing;
    using Xunit;
    using Template.Api.Controllers;
    using Template.Contracts.Responses;
    using Template.Infrastructure.Queries;
    using Moq;
    using AutoFixture.Xunit2;

    public class AggregateNameControllerTests
    {
        private readonly Mock<IGetAllAggregateNamesQuery> getAllAggregateNamesQueryMock;
        private readonly Mock<IGetAggregateNameByIdQuery> getAggregateNameByIdQueryMock;

        private readonly AggregateNameController sut;

        public AggregateNameControllerTests()
        {
            var messageSession = new TestableMessageSession();
            getAllAggregateNamesQueryMock = new Mock<IGetAllAggregateNamesQuery>();
            getAggregateNameByIdQueryMock = new Mock<IGetAggregateNameByIdQuery>();

            sut = new AggregateNameController(messageSession, getAllAggregateNamesQueryMock.Object, getAggregateNameByIdQueryMock.Object);
        }

        [Theory]
        [AutoData]
        public void Get_ShouldReturnOkAndAggregateNames(List<AggregateName> aggregateNames)
        {
            // Arrange
            getAllAggregateNamesQueryMock
                .Setup(x => x.ExecuteAsync())
                .ReturnsAsync(aggregateNames);

            // Act
            var response = sut.Get();

            // Assert
            response.Should().NotBeNull();
            var okObjectResult = response.Result as OkObjectResult;
            okObjectResult.Should().NotBeNull();
            okObjectResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            okObjectResult.Value.Should().NotBeNull();
            okObjectResult.Value.Should().BeEquivalentTo(aggregateNames);

            getAllAggregateNamesQueryMock.Verify(x => x.ExecuteAsync(), Times.Once);
        }

        [Fact]
        public async Task Get_GivenADefaultGuid_ShouldThrowException()
        {
            // Arrange
            var testValue = Guid.Empty;
            var defaultMessage = "Guid value cannot be default";
            var parameterName = "id";

            Func<Task> func = async () => await sut.Get(testValue);

            // Act & Assert
            var exception = await func.Should().ThrowAsync<ArgumentException>();
            exception.And.Message.Should().Contain(defaultMessage);
            exception.Which.ParamName.Should().Be(parameterName);
        }

        [Theory]
        [AutoData]
        public void Get_GivenRecordWithIdExists_ShouldReturnOkAndAggregateName(AggregateName aggregateName)
        {
            // Arrange
            getAggregateNameByIdQueryMock
                .Setup(x => x.ExecuteAsync(aggregateName.Id))
                .ReturnsAsync(aggregateName);

            // Act
            var response = sut.Get(aggregateName.Id);

            // Assert
            response.Should().NotBeNull();
            var okObjectResult = response.Result as OkObjectResult;
            okObjectResult.Should().NotBeNull();
            okObjectResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            okObjectResult.Value.Should().NotBeNull();
            okObjectResult.Value.Should().BeEquivalentTo(aggregateName);
        }
    }
}
