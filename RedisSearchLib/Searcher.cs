using NRedisStack;
using NRedisStack.RedisStackCommands;
using NRedisStack.Search;
using StackExchange.Redis;

namespace RedisSearchLib
{
    public interface ISearcher
    {
        Task<SearchResult?> SearchProductsAsync(string keywords);
        Task<SearchResult?> SearchTemplatesAsync(string keywords);
    }

    public class Searcher : ISearcher
    {
        private readonly IDatabase db;

        public Searcher(IDatabase db)
        {
            this.db = db;
        }

        public async Task<SearchResult?> SearchTemplatesAsync(string keywords)
        {
            if(!string.IsNullOrEmpty(keywords))
            {
                keywords = keywords.Replace(' ','|');   //use an OR separator. Default is to use AND.
            }
            SearchCommands ft = db.FT();
            var result = await ft.SearchAsync(Constants.TemplateIndexName, new Query($"{keywords}"));   //by default all text fields are searched
            return result;
        }
        public async Task<SearchResult?> SearchProductsAsync(string keywords)
        {
            if (!string.IsNullOrEmpty(keywords))
            {
                keywords = keywords.Replace(' ', '|');   //use an OR separator. Default is to use AND.
            }
            SearchCommands ft = db.FT();
            var result = await ft.SearchAsync(Constants.ProductIndexName, new Query($"{keywords}"));    //by default all text fields are searched
            return result;
        }
    }
}
