using Redis.OM.Modeling;

namespace RedisSearchOM
{
    [Document(StorageType = StorageType.Json, Prefixes = new[] { "template" }, IndexName = "templates-idx")]
    public record TemplateDTO
    {
        [RedisIdField]
        public Ulid RedisId { get; set; }
        [Indexed]
        public int Id { get; set; }
        [Searchable]
        public required string Name { get; set; }
        [Indexed]
        public bool LimitedToStores { get; set; }
        [Indexed]
        public required int[] AvailableForStoreIds { get; set; }
        [Indexed]
        public bool Published { get; set; }
        [Indexed]
        public bool VisibleIndividually { get; set; }
        [Indexed]
        public bool Deleted { get; set; }
        /// <summary>
        /// This is the ID of the Related Product that represents the Base Product in the product model in NOP.
        /// </summary>
        [Indexed]
        public int? BaseProductId { get; set; }
        /// <summary>
        /// This is the ID of the Child Product (Size/Material) represented by the Size Product Attribute's Associated Product Id
        /// </summary>
        [Indexed]
        public int? ChildProductId { get; set; }
        [Searchable]
        public string? Tags { get; set; }
        [Indexed]
        public IList<SpecificationAttributeDTO>? SpecificationAttributes { get; set; }
    }

    public record SpecificationAttributeDTO : IEquatable<SpecificationAttributeDTO>
    {
        [Indexed]
        public int Id { get; set; }
        [Indexed]
        public required string Name { get; set; }
        [Indexed]
        public bool AllowFiltering { get; set; }
        [Indexed]
        public bool ShowOnProductPage { get; set; }
        [Indexed]
        public int DisplayOrder { get; set; }
        [Indexed]
        public int OptionId { get; set; }
        [Indexed]
        public required string Value { get; set; }
        [Indexed]
        public string? ColorSquaresRgb { get; set; }

        public virtual bool Equals(SpecificationAttributeDTO other)
        {
            return other != null && Id == other.Id && OptionId == other.OptionId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, OptionId);
        }
    }
}
