using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Template.Contracts.Requests;
using Template.Contracts.Responses;

namespace Template.Api.Controllers
{
    using NServiceBus;
    using System.Threading.Tasks;
    using Template.Contracts.Messages.Commands;
    using Template.Infrastructure.Messages.Responses;
    using Template.Infrastructure.Queries;

    [Route("api/[controller]")]
    [ApiController]
    public class AggregateNameController : ControllerBase
    {
        private readonly IMessageSession messageSession;
        private readonly IGetAllAggregateNamesQuery getAllAggregateNamesQuery;
        private readonly IGetAggregateNameByIdQuery getAggregateNameByIdQuery;

        public AggregateNameController(IMessageSession messageSession, IGetAllAggregateNamesQuery getAllAggregateNamesQuery, IGetAggregateNameByIdQuery getAggregateNameByIdQuery)
        {
            this.messageSession = messageSession;
            this.getAllAggregateNamesQuery = getAllAggregateNamesQuery;
            this.getAggregateNameByIdQuery = getAggregateNameByIdQuery;
        }

        // GET api/aggreatename
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await this.getAllAggregateNamesQuery.ExecuteAsync();
            return Ok(result);
        }

        // GET api/aggregatename/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await getAggregateNameByIdQuery.ExecuteAsync(id);
            return result == null ? (ActionResult)NotFound() : Ok(result);
        }

        // POST api/aggreatename
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateAggregateNameRequest request)
        {
            var createAggregateNameCommand =
                new CreateAggregateName(request.Id);

            var response = await this.messageSession.Request<RequestResponse>(createAggregateNameCommand);
            if (!response.Success)
            {
                throw response.Exception;
            }

            return Ok();
        }

        // PUT api/aggreatename/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] string value)
        {
            var updateAggregateNameCommand =
                new UpdateAggregateName(id);

            var response = await this.messageSession.Request<RequestResponse>(updateAggregateNameCommand);
            if (!response.Success)
            {
                throw response.Exception;
            }

            return NoContent();
        }

        // DELETE api/aggreatename/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleteAggregateNameCommand = new DeleteAggregateName(id);
            await this.messageSession.Send(deleteAggregateNameCommand);

            return NoContent();
        }
    }
}
