namespace ArtForAll.Events.Consumer.repositories
{
    using ArtForAll.Events.Consumer.Entities;
    using ArtForAll.Events.Infrastructure.DynamoRepositories.Entities;
    using ArtForAll.Shared.Contracts.DDD;
    using ArtForAll.Shared.ErrorHandler;

    public interface IEventsRepositoryPersistenceModel
    {
        Task<Result<Event, Error>> Update(Event simpleEvent);
        Task<Result> UpdateState(string pk, string newState);
        Task<Result> PatchEvent(EventPatch @event);
        Task<Result> DeleteASync(string pk);
        Task<Result<Event, Error>> AddImageAsync(string eventId, Image image);
        Task<Result<Event, Error>> AddEventAsync(Event simpleEvent);
    }
}
