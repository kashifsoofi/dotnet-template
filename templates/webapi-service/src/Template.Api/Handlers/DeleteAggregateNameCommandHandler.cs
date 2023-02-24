//namespace Template.Api.Handlers
//{
//    using System;
//    using System.Linq;
//    using System.Threading.Tasks;
//    using NServiceBus;
//    using Template.Contracts.Messages.Commands;
//    using Template.Domain.Aggregates.AggregateName;

//    public class DeleteAggregateNameCommandHandler : IHandleMessages<DeleteAggregateName>
//    {
//        private readonly IAggregateNameAggregateReadRepository aggregateReadRepository;
//        private readonly IAggregateNameAggregateWriteRepository aggregateWriteRepository;

//        public DeleteAggregateNameCommandHandler(IAggregateNameAggregateReadRepository aggregateReadRepository, IAggregateNameAggregateWriteRepository aggregateWriteRepository)
//        {
//            this.aggregateReadRepository = aggregateReadRepository;
//            this.aggregateWriteRepository = aggregateWriteRepository;
//        }

//        public async Task Handle(DeleteAggregateName command, IMessageHandlerContext context)
//        {
//            if (command == null || command.Id == Guid.Empty)
//            {
//                throw new ArgumentException(nameof(command));
//            }

//            try
//            {
//                var aggregate = await this.aggregateReadRepository.GetByIdAsync(command.Id);
//                aggregate.Delete();

//                await PersistAndPublishAsync(aggregate, context);
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e);
//                throw;
//            }
//        }

//        private async Task PersistAndPublishAsync(IAggregateNameAggregate aggregate, IMessageHandlerContext context)
//        {
//            await aggregateWriteRepository.DeleteAsync(aggregate);

//            await Task.WhenAll(aggregate.UncommittedEvents.Select(async (x) => await context.Publish(x)));
//        }
//    }
//}