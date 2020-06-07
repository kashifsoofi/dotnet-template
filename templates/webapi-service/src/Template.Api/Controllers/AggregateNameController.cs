using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Template.Contracts.Requests;
using Template.Contracts.Responses;

namespace Template.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AggregateNameController : ControllerBase
    {
        // GET api/aggreatename
        [HttpGet]
        public IActionResult Get()
        {
            var aggregatenames = new List<AggregateName>
            {
                new AggregateName { Id = Guid.NewGuid(), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow },
                new AggregateName { Id = Guid.NewGuid(), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow },
            };
            return Ok(aggregatenames);
        }

        // GET api/aggregatename/5
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var aggregatename = new AggregateName { Id = id, CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow };
            return Ok(aggregatename);
        }

        // POST api/aggreatename
        [HttpPost]
        public IActionResult Post([FromBody] CreateAggregateNameRequest request)
        {
            var aggregatename = new AggregateName { Id = request.Id, CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow };
            return CreatedAtAction("Get", new { id = aggregatename.Id }, aggregatename);
        }

        // PUT api/aggreatename/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            return NoContent();
        }

        // DELETE api/aggreatename/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return NoContent();
        }
    }
}
