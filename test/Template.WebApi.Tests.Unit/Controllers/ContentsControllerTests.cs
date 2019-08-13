using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Template.Domain.Responses;
using Xunit;
using Template.WebApi.Controllers;

namespace Template.WebApi.Tests.Unit.Controllers
{
    public class ContentsControllerTests
    {
        private readonly ContentsController _sut;

        public ContentsControllerTests()
        {
            _sut = new ContentsController();
        }

        [Fact]
        public void Get_Should_return_ok_with_content()
        {
            var result = _sut.Get(1);

            result.Should().NotBeNull();
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var content = okResult.Value.Should().BeAssignableTo<Content>().Subject;

            content.Value.Should().Be("value");
        }
    }
}
