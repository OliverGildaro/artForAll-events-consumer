using ArtForAll.Events.Consumer.Entities.Helpers;
using System.Text.Json.Serialization;

namespace ArtForAll.Events.Consumer.Entities
{
    public class EventPatch
    {
        [JsonPropertyName("pk")]
        public string Pk { get; set; }
        [JsonPropertyName("sk")]
        public string CreatedAt { get; set; }
        public IEnumerable<EventPatchOperation> PatchOperations { get; set; } = new List<EventPatchOperation>();
    }
}
