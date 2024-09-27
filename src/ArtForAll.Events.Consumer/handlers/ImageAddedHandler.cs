using ArtForAll.Events.Consumer.mapper;
using ArtForAll.Events.Consumer.Messages;
using ArtForAll.Events.Consumer.repositories;
using ArtForAll.Shared.Contracts.CQRS;
using ArtForAll.Shared.ErrorHandler;

namespace ArtForAll.Events.Consumer.handlers
{
    public class ImageAddedHandler : ICommandHandler<ImageAdded, Result>
    {
        private readonly IEventsRepository repository;

        public ImageAddedHandler(IEventsRepository repository)
        {
            this.repository = repository;
        }
        public async Task<Result> HandleAsync(ImageAdded command)
        {
            var result = await this.repository.AddImageAsync(command.EventId, command.CreatedAt, command.EventImageToDynamoEventImage());

            if (result.IsFailure)
            {
                return Result.Failure("");
            }

            return Result.Success();
        }
    }
}
