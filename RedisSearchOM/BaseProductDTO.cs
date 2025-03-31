using Redis.OM.Modeling;

namespace RedisSearchOM
{
    [Document(StorageType = StorageType.Json, Prefixes = new[] { "product" }, IndexName = "products-idx")]
    public record BaseProductDTO
    {
        [RedisIdField]
        public Ulid RedisId { get; set; }
        [Indexed]
        public int Id { get; set; }
        [Searchable]
        public required string Name { get; set; }
        [Searchable]
        public string? ShortDescription { get; set; }
        [Searchable]
        public string? FullDescription { get; set; }
        [Indexed]
        public int DisplayOrder { get; set; }
        [Indexed]
        public bool Published { get; set; }
        [Indexed]
        public bool VisibleIndividually { get; set; }
        [Indexed]
        public bool Deleted { get; set; }
        [Indexed]
        public bool LimitedToStores { get; set; }
        [Indexed]
        public required int[] AvailableForStoreIds { get; set; }
        [Searchable]
        public string? Categories { get; set; }
        [Indexed]
        public string? CategoryIds { get; set; }
        [Indexed]
        public required string ProductTemplate { get; set; }
        //Sales Volume
        [Indexed]
        public int Popularity { get; set; }
        [Searchable]
        public Dictionary<string, string>? Specs { get; set; }
        [Searchable]
        public string? Tags { get; set; }
    }
}
