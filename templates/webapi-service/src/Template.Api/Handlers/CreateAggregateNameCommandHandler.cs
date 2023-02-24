//namespace Template.Api.Handlers
//{
//    using System;
//    using System.Linq;
//    using System.Threading.Tasks;
//    using NServiceBus;
//    using Template.Contracts.Messages.Commands;
//    using Template.Domain.Aggregates.AggregateName;

//    public class CreateAggregateNameCommandHandler : IHandleMessages<CreateAggregateName>
//    {
//        private readonly IAggregateNameAggregateReadRepository aggregateReadRepository;
//        private readonly IAggregateNameAggregateWriteRepository aggregateWriteRepository;

//        public CreateAggregateNameCommandHandler(IAggregateNameAggregateReadRepository aggregateReadRepository, IAggregateNameAggregateWriteRepository aggregateWriteRepository)
//        {
//            this.aggregateReadRepository = aggregateReadRepository;
//            this.aggregateWriteRepository = aggregateWriteRepository;
//        }

//        public async Task Handle(CreateAggregateName command, IMessageHandlerContext context)
//        {
//            if (command == null)
//            {
//                throw new ArgumentNullException(nameof(command));
//            }

//            if (command.Id == Guid.Empty)
//            {
//                throw new ArgumentException("Guid value cannot be default", nameof(command.Id));
//            }

//            try
//            {
//                var aggregate = await this.aggregateReadRepository.GetByIdAsync(command.Id);
//                aggregate.Create(command);

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
//            if (aggregate.IsNew)
//            {
//                await aggregateWriteRepository.CreateAsync(aggregate);
//            }
//            else
//            {
//                await aggregateWriteRepository.UpdateAsync(aggregate);
//            }

//            await Task.WhenAll(aggregate.UncommittedEvents.Select(async (x) => await context.Publish(x)));
//        }
//    }
//}