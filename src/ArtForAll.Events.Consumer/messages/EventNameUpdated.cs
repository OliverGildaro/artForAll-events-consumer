using ArtForAll.Events.Consumer.messages;
using ArtForAll.Events.Infrastructure.DynamoRepositories.Entities;
using ArtForAll.Shared.Contracts.CQRS;

namespace ArtForAll.Events.Consumer.Messages
{
    public class EventNameUpdated : ICommand
    {
        public string Name { get; set; }
        public string State { get; set; }
        public Tuple<string, string> PrevPK { get; set; }

    }
}
