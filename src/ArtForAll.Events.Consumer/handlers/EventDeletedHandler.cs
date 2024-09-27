
namespace ArtForAll.Events.Consumer.handlers
{
    using ArtForAll.Events.Consumer.Messages;
    using ArtForAll.Events.Consumer.repositories;
    using ArtForAll.Shared.Contracts.CQRS;
    using ArtForAll.Shared.ErrorHandler;

    public class EventDeletedHandler : ICommandHandler<EventDeleted, Result>
    {
        private readonly IEventsRepository repository;

        public EventDeletedHandler(IEventsRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Result> HandleAsync(EventDeleted command)
        {
            var result = await this.repository.DeleteASync(command.Id, command.CreatedAt);

            if (result.IsFailure)
            {
                return Result.Failure("");
            }

            return Result.Success();
        }
    }
}
