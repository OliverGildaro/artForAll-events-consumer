using Amazon.DynamoDBv2.DocumentModel;
using ArtForAll.Events.Infrastructure.DynamoRepositories.Entities;

namespace ArtForAll.Events.Infrastructure.DynamoRepositories.Mapper
{
    public class DocMapper
    {
        internal Event ToEntity(Document item)
        {
            return ToEventContract(item);
        }

        internal IEnumerable<Event> ToEntities(List<Document> items)
        {
            return items.Select(ToEventContract);
        }

        public Event ToEventContract(Document item)
        {
            return new Event
            {
                Pk = item["pk"],
                Name = item["name"],
                Description = item["Description"],
                Date = item["Date"],
                State = item["state"],
                Type = item["type"],
            };
        }
    }
}
