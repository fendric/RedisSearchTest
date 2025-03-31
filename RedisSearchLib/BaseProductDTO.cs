using Newtonsoft.Json;

namespace RedisSearchLib
{
    public record BaseProductDTO
    {
        [JsonProperty]
        public int Id { get; set; }
        [JsonProperty]
        public required string Name { get; set; }
        [JsonProperty]
        public string? ShortDescription { get; set; }
        [JsonProperty]
        public string? FullDescription { get; set; }
        [JsonProperty]
        public int DisplayOrder { get; set; }
        [JsonProperty]
        public bool Published { get; set; }
        [JsonProperty]
        public bool VisibleIndividually { get; set; }
        [JsonProperty]
        public bool Deleted { get; set; }
        [JsonProperty]
        public bool LimitedToStores { get; set; }
        [JsonProperty]
        public required int[] AvailableForStoreIds { get; set; }
        [JsonProperty]
        public string? Categories { get; set; }
        [JsonProperty]
        public string? CategoryIds { get; set; }
        [JsonProperty]
        public required string ProductTemplate { get; set; }
        //Sales Volume
        [JsonProperty]
        public int Popularity { get; set; }
        [JsonProperty]
        public Dictionary<string, string>? Specs { get; set; }
        [JsonProperty]
        public string? Tags { get; set; }
    }
}
