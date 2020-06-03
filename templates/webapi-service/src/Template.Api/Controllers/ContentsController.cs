using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Template.Domain.Requests;
using Template.Domain.Responses;

namespace Template.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentsController : ControllerBase
    {
        // GET api/contents
        [HttpGet]
        public IActionResult Get()
        {
            var contents = new List<Content>
            {
                new Content(1, "value1"),
                new Content(2, "value2")
            };
            return Ok(new GetContentsResponse(contents));
        }

        // GET api/contents/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var content = new Content(id, $"value{id}");
            return Ok(content);
        }

        // POST api/contents
        [HttpPost]
        public IActionResult Post([FromBody] CreateContentRequest request)
        {
            var content = new Content(request.Id, request.Value);
            return CreatedAtAction("Get", new { id = content.Id }, content);
        }

        // PUT api/contents/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            return NoContent();
        }

        // DELETE api/contents/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return NoContent();
        }
    }
}
