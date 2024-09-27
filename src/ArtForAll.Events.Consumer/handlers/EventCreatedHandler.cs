namespace ArtForAll.Events.Consumer.handlers
{
    using ArtForAll.Events.Consumer.mapper;
    using ArtForAll.Events.Consumer.Messages;
    using ArtForAll.Events.Consumer.repositories;
    using ArtForAll.Shared.Contracts.CQRS;
    using ArtForAll.Shared.ErrorHandler;
    using System.Threading.Tasks;

    public class EventCreatedHandler : ICommandHandler<EventCreated, Result>
    {
        private readonly IEventsRepository repository;

        public EventCreatedHandler(IEventsRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Result> HandleAsync(EventCreated command)
        {
            var result = await this.repository.Update(command.EventCreatedToDynamoEvent());

            if (result.IsFailure)
            {
                return Result.Failure("");
            }

            return Result.Success();
        }
    }
}
