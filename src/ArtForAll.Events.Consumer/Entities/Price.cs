using Newtonsoft.Json;

namespace ArtForAll.Events.Consumer.Entities
{
    public class Price
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? CurrencyExchange { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float? MonetaryValue { get; set; }
    }
}
