﻿using ArtForAll.Events.Consumer.Entities;
using Newtonsoft.Json;
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
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        [JsonPropertyName("sk")]
        public string CreatedAt { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Capacity { get; set; }
        public Image Image { get; set; }
        public Address Addres { get; set; }
        public Price Price { get; set; }
    }
}
