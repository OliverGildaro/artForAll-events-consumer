namespace ArtForAll.Events.Consumer.handlers
{
    using ArtForAll.Events.Consumer.Messages;
    using ArtForAll.Events.Consumer.repositories;
    using ArtForAll.Shared.Contracts.CQRS;
    using ArtForAll.Shared.ErrorHandler;

    public class EventNameUpdatedHandler : ICommandHandler<EventNameUpdated, Result>
    {
        private readonly IEventsRepository repository;

        public EventNameUpdatedHandler(IEventsRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Result> HandleAsync(EventNameUpdated command)
        {
            var name = command.PrevPK.Item2;
            var state = command.PrevPK.Item1;
            var eventResult = await this.repository.FindAsync(state, name);
            var @event = eventResult.Value;
            @event.Name = command.Name;
            @event.State = command.State;

            if (eventResult.IsFailure)
            {
                return Result.Failure("");
            }
            await this.repository.DeleteASync(state, name);
            var result = await this.repository.Update(@event);

            if (result.IsFailure)
            {
                return Result.Failure("");
            }

            return Result.Success();
        }
    }
}
