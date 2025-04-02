using StackExchange.Redis;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using NRedisStack.Search;
using NRedisStack.Search.Literals.Enums;

namespace RedisSearchLib
{
    public interface IIndexer
    {
        Task<bool> CreateProductIndexAsync(string indexName, string keyPrefix);
        Task<bool> CreateTemplateIndexAsync(string indexName, string keyPrefix);
    }

    public class Indexer : IIndexer
    {
        private readonly IDatabase db;

        public Indexer(IDatabase db)
        {
            this.db = db;
        }

        public async Task<bool> CreateProductIndexAsync(string indexName, string keyPrefix)
        {
            SearchCommands ft = db.FT();
            try { await ft.DropIndexAsync(indexName); } catch { }
            return await ft.CreateAsync(indexName, new FTCreateParams().On(IndexDataType.JSON).Prefix($"{keyPrefix}"),
                new Schema().AddNumericField(new FieldName("$.Id", "Id"))
                            .AddTextField(new FieldName("$.Name", "Name"))
                            .AddTextField(new FieldName("$.ShortDescription", "ShortDescription"))
                            .AddTextField(new FieldName("$.FullDescription", "FullDescription"))
                            .AddNumericField(new FieldName("$.DisplayOrder", "DisplayOrder"))
                            .AddTagField(new FieldName("$.Published", "Published"))
                            .AddTagField(new FieldName("$.Deleted", "Deleted"))
                            .AddTagField(new FieldName("$.VisibleIndividually", "VisibleIndividually"))
                            .AddTagField(new FieldName("$.LimitedToStores", "LimitedToStores"))
                            .AddTextField(new FieldName("$.Categories", "Categories"))
                            .AddTextField(new FieldName("$.CategoryIds", "CategoryIds"))
                            .AddTagField(new FieldName("$.ProductTemplate", "ProductTemplate"))
                            .AddNumericField(new FieldName("$.Popularity", "Popularity"))
                            .AddTextField(new FieldName("$.Tags", "Tags"))
                            .AddTagField(new FieldName("$.AvailableForStoreIds", "AvailableForStoreIds"))
                            .AddTagField(new FieldName("$.Specs", "Specs"))
                );
        }
        public async Task<bool> CreateTemplateIndexAsync(string indexName, string keyPrefix)
        {
            SearchCommands ft = db.FT();
            try { await ft.DropIndexAsync(indexName); } catch { }
            return await ft.CreateAsync(indexName, new FTCreateParams().On(IndexDataType.JSON).Prefix($"{keyPrefix}"),
                new Schema().AddNumericField(new FieldName("$.Id", "Id"))
                            .AddTextField(new FieldName("$.Name", "Name"))
                            .AddTagField(new FieldName("$.Published", "Published"))
                            .AddTagField(new FieldName("$.Deleted", "Deleted"))
                            .AddTagField(new FieldName("$.VisibleIndividually", "VisibleIndividually"))
                            .AddTagField(new FieldName("$.LimitedToStores", "LimitedToStores"))
                            .AddTagField(new FieldName("$.BaseProductId", "BaseProductId"))
                            .AddTagField(new FieldName("$.ChildProductId", "ChildProductId"))
                            .AddTextField(new FieldName("$.Tags", "Tags"))
                            .AddTagField(new FieldName("$.SpecificationAttributes", "SpecificationAttributes"))
                );
        }
    }
}
