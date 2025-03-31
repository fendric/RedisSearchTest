﻿using NRedisStack;
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
            SearchCommands ft = db.FT();
            var result = await ft.SearchAsync(Constants.TemplateIndexName, new Query($"@Name:({keywords}) @ShortDescription:({keywords}) @FullDescription:({keywords}) @Categories:({keywords}) @Tags:({keywords})"));
            return result;
        }
        public async Task<SearchResult?> SearchProductsAsync(string keywords)
        {
            SearchCommands ft = db.FT();
            var result = await ft.SearchAsync(Constants.ProductIndexName, new Query($"@Name:({keywords}) @Tags:({keywords})"));
            return result;
        }
    }
}
