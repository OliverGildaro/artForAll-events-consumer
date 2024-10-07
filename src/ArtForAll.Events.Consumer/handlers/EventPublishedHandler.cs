
namespace ArtForAll.Events.Consumer.handlers
{
    using ArtForAll.Events.Consumer.mapper;
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
            var name = command.PrevPK.Item1;
            var state = command.PrevPK.Item2;
            var res= await this.repository.DeleteASync(state, name);
            if (res.IsFailure)
            {
                return Result.Failure("");
            }
            var result = await this.repository.Update(command.EventPublishedToDynamoEvent());

            if (result.IsFailure)
            {
                return Result.Failure("");
            }

            return Result.Success();
        }
    }
}
