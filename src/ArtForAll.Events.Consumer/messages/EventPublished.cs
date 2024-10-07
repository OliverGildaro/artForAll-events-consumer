using ArtForAll.Events.Consumer.messages;
using ArtForAll.Events.Infrastructure.DynamoRepositories.Entities;
using ArtForAll.Shared.Contracts.CQRS;

namespace ArtForAll.Events.Consumer.Messages
{
    public class EventPublished : ICommand
    {
        public string Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CreatedAt { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string StateEvent { get; set; }
        public string Type { get; set; }
        public int? Capacity { get; set; }
        public Address Addres { get; set; }
        public Price Price { get; set; }
        public Tuple<string, string> PrevPK { get; set; }

    }
}
