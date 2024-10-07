namespace ArtForAll.Events.Consumer.repositories
{
    using ArtForAll.Events.Consumer.Entities;
    using ArtForAll.Events.Infrastructure.DynamoRepositories.Entities;
    using ArtForAll.Shared.Contracts.DDD;
    using ArtForAll.Shared.ErrorHandler;

    public interface IEventsRepository
    {
        Task<Result<Event, Error>> Update(Event simpleEvent);
        Task<Result> UpdateState(string pk, string createdAt, string newState);
        Task<Result> PatchEvent(EventPatch @event);
        Task<Result> DeleteASync(string state, string name);
        Task<Result<Event, Error>> FindAsync(string state, string name);
        Task<Result<Event, Error>> AddImageAsync(string eventId, string createdAt, Image image);
    }
}
