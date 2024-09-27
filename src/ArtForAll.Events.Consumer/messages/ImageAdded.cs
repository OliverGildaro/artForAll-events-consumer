using ArtForAll.Shared.Contracts.CQRS;

namespace ArtForAll.Events.Consumer.Messages
{
    public class ImageAdded : ICommand
    {
        public string Id { get; set; }
        public string CreatedAt { get; set; }
        public string EventId { get; set; }
        public string contentType { get; set; }
        public string fileName { get; set; }
    }
}
