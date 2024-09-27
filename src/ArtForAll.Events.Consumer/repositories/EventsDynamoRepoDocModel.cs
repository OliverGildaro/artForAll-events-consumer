using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using ArtForAll.Events.Infrastructure.DynamoRepositories.Mapper;

namespace ArtForAll.Events.Consumer.repositories
{
    public class EventsDynamoRepoDocModel 
    {
        private const string TableName = "eventsDB";
        private readonly Table _table;

        private readonly DocMapper mapper;

        public EventsDynamoRepoDocModel(IAmazonDynamoDB amazonDynamoClient, DocMapper mapper)
        {
            _table = Table.LoadTable(amazonDynamoClient, TableName);
            this.mapper = mapper;
        }

        public async Task AddEventAsync(Document simpleEvent)
        {
            await _table.PutItemAsync(simpleEvent);
        }
    }
}
