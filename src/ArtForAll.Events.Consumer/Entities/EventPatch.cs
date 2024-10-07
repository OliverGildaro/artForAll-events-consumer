using ArtForAll.Events.Consumer.Entities.Helpers;
using System.Text.Json.Serialization;

namespace ArtForAll.Events.Consumer.Entities
{
    public class EventPatch
    {
        [JsonPropertyName("pk")]
        public string State { get; set; }
        [JsonPropertyName("sk")]
        public string Name { get; set; }
        public IEnumerable<EventPatchOperation> PatchOperations { get; set; } = new List<EventPatchOperation>();
    }
}
