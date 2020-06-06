using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Template.Api.Controllers;
using Template.Domain.Responses;

namespace Template.Api.Tests.Unit.Controllers
{
    public class AggregateNameControllerTests
    {
        private readonly AggregateNameController _sut;

        public AggregateNameControllerTests()
        {
            _sut = new AggregateNameController();
        }

        [Fact]
        public void Get_should_return_ok_with_content()
        {
            var result = _sut.Get(1);

            result.Should().NotBeNull();
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var content = okResult.Value.Should().BeAssignableTo<Content>().Subject;

            content.Value.Should().Be("value1");
        }
    }
}
