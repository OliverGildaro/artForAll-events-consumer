
namespace ArtForAll.Events.Consumer.handlers
{
    using ArtForAll.Events.Consumer.Messages;
    using ArtForAll.Events.Consumer.repositories;
    using ArtForAll.Shared.Contracts.CQRS;
    using ArtForAll.Shared.ErrorHandler;

    public class EventPublishedHandler : ICommandHandler<EventPublished, Result>
    {
        private readonly IEventsRepository repository;

        public EventPublishedHandler(IEventsRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Result> HandleAsync(EventPublished command)
        {
            var result = await this.repository.UpdateState(command.Id, command.CreatedAt, command.StateEvent);

            if (result.IsFailure)
            {
                return Result.Failure("");
            }

            return Result.Success();
        }
    }
}
