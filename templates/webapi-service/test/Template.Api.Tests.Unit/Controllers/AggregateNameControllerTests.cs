namespace Template.Api.Tests.Unit.Controllers
{
    using System;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;
    using Template.Api.Controllers;
    using Template.Contracts.Responses;

    public class AggregateNameControllerTests
    {
        private readonly AggregateNameController _sut;

        public AggregateNameControllerTests()
        {
            _sut = new AggregateNameController();
        }

        [Fact]
        public void Get_should_return_ok_with_AggregateName()
        {
            var id = Guid.NewGuid();
            var result = _sut.Get(id);

            result.Should().NotBeNull();
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var aggregatename = okResult.Value.Should().BeAssignableTo<AggregateName>().Subject;

            aggregatename.Id.Should().Be(id);
        }
    }
}
