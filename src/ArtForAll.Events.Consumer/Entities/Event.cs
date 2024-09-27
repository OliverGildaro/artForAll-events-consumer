using ArtForAll.Events.Consumer.Entities;
using System.Text.Json.Serialization;

namespace ArtForAll.Events.Infrastructure.DynamoRepositories.Entities
{
    public class Event
    {
        [JsonPropertyName("pk")]
        public string Pk { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string State { get; set; }
        public string Date { get; set; }
        [JsonPropertyName("sk")]
        public string CreatedAt { get; set; }
        public Image Image { get; set; }
    }
}
