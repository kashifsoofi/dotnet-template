namespace Template.Host.Handlers
{
    using NServiceBus;
    using System.Threading.Tasks;
    using Template.Contracts.Requests;
    using Template.Infrastructure.Messages.Responses;

    internal class CreateAggregateNameRequestHandler : IHandleMessages<CreateAggregateNameRequest>
    {
        public async Task Handle(CreateAggregateNameRequest message, IMessageHandlerContext context)
        {
            var response = new RequestResponse();
            response.Success = true;

            await context.Reply(response);
        }
    }
}
