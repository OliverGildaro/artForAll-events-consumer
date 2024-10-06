namespace ArtForAll.Events.Consumer.Messages
{
    using ArtForAll.Events.Consumer.messages;
    using ArtForAll.Shared.Contracts.CQRS;
    using System.Net;

    public class EventCreated : ICommand
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

    }
}
