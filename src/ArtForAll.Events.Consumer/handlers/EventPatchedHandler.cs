using ArtForAll.Events.Consumer.mapper;
using ArtForAll.Events.Consumer.Messages;
using ArtForAll.Events.Consumer.repositories;
using ArtForAll.Shared.Contracts.CQRS;
using ArtForAll.Shared.ErrorHandler;

namespace ArtForAll.Events.Consumer.handlers
{
    public class EventPatchedHandler : ICommandHandler<EventPatched, Result>
    {
        private readonly IEventsRepository repository;

        public EventPatchedHandler(IEventsRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Result> HandleAsync(EventPatched command)
        {
            var result = await this.repository.PatchEvent(command.EventPatchedToDynamoEvent());

            if (result.IsFailure)
            {
                return Result.Failure("");
            }

            return Result.Success();
        }
    }
}
