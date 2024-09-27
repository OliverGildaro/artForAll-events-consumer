namespace ArtForAll.Events.Consumer.Messages
{
    using ArtForAll.Shared.Contracts.CQRS;

    public class EventCreated : ICommand
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string StateEvent { get; set; }
        public string Date { get; set; }
        public string CreatedAt { get; set; }

    }
}
