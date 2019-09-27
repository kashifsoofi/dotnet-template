using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Template.Domain.Responses;

namespace Template.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentsController : ControllerBase
    {
        // GET api/contents
        [HttpGet]
        public async Task<IActionResult> Get()
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
            var content = new Content(id, "value1");
            return Ok(content);
        }

        // POST api/contents
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Content request)
        {
            return CreatedAtAction("Get", new { id = request.Id }, request);
        }

        // PUT api/contents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] string value)
        {
            return NoContent();
        }

        // DELETE api/contents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return NoContent();
        }
    }
}
