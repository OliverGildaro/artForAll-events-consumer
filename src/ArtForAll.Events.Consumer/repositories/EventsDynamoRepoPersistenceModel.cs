using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using ArtForAll.Events.Consumer.Entities;
using ArtForAll.Events.Infrastructure.DynamoRepositories.Entities;
using ArtForAll.Shared.Contracts.DDD;
using ArtForAll.Shared.ErrorHandler;

namespace ArtForAll.Events.Consumer.repositories
{
    public class EventsDynamoRepoPersistenceModel : IEventsRepositoryPersistenceModel
    {
        private readonly DynamoDBContext dBContext;

        public EventsDynamoRepoPersistenceModel(IAmazonDynamoDB amazonDynamoClient)
        {
            this.dBContext = new DynamoDBContext(amazonDynamoClient);
        }

        public async Task<Result<Event, Error>> AddEventAsync(Event simpleEvent)
        {
            await dBContext.SaveAsync(simpleEvent);
            return Result<Event, Error>.Success(simpleEvent);
        }

        public Task<Result<Event, Error>> AddImageAsync(string eventId, Image image)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteASync(string pk)
        {
            throw new NotImplementedException();
        }

        public Task<Result> PatchEvent(EventPatch @event)
        {
            throw new NotImplementedException();
        }

        //we are using this to patch
        //Based on the tutorial, this method willonly update the changed attributtes
        public async Task<Result<Event, Error>> Update(Event simpleEvent)
        {
            await dBContext.SaveAsync(simpleEvent);
            return Result<Event, Error>.Success(simpleEvent);
        }

        public Task<Result> UpdateState(string pk, string newState)
        {
            throw new NotImplementedException();
        }
    }
}
