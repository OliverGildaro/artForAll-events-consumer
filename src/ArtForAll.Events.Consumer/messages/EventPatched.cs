using ArtForAll.Events.Consumer.Entities.Helpers;
using ArtForAll.Shared.Contracts.CQRS;
using System.Text.Json.Serialization;
namespace ArtForAll.Events.Consumer.Messages
{
    public class EventPatched : ICommand
    {
        public string Id { get; set; }
        public string CreatedAt { get; set; }
        public IEnumerable<EventPatchOperation> PatchOperations { get; set; } = new List<EventPatchOperation>();
    }
}
