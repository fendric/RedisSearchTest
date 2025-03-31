namespace RedisSearchLib
{
    public record TemplateDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool LimitedToStores { get; set; }
        public required int[] AvailableForStoreIds { get; set; }
        public bool Published { get; set; }
        public bool VisibleIndividually { get; set; }
        public bool Deleted { get; set; }
        /// <summary>
        /// This is the ID of the Related Product that represents the Base Product in the product model in NOP.
        /// </summary>
        public int? BaseProductId { get; set; }
        /// <summary>
        /// This is the ID of the Child Product (Size/Material) represented by the Size Product Attribute's Associated Product Id
        /// </summary>
        public int? ChildProductId { get; set; }
        public string? Tags { get; set; }
        public IList<SpecificationAttributeDTO>? SpecificationAttributes { get; set; }
    }

    public record SpecificationAttributeDTO : IEquatable<SpecificationAttributeDTO>
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool AllowFiltering { get; set; }
        public bool ShowOnProductPage { get; set; }
        public int DisplayOrder { get; set; }
        public int OptionId { get; set; }
        public required string Value { get; set; }
        public string? ColorSquaresRgb { get; set; }

        public virtual bool Equals(SpecificationAttributeDTO other)
        {
            return other != null && Id == other.Id && this.OptionId == other.OptionId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, OptionId);
        }
    }
}
