// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Redis.OM;
using RedisSearchLib;
using RedisSearchOM;
using StackExchange.Redis;

var services = new ServiceCollection();
//for NRedisStack
services.AddScoped<IDataLoader,DataLoader>();
services.AddScoped<IIndexer, Indexer>();
services.AddScoped<ISearcher, Searcher>();
services.AddScoped(cfg =>
{
    IConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect("localhost:6380");
    return multiplexer.GetDatabase();
});
//for Redis.OM
services.AddScoped<IOmDataLoader, OmDataLoader>();
services.AddScoped<IOmSearcher, OmSearcher>();
services.AddScoped<IIndexCreationService, IndexCreationService>();
services.AddSingleton(new RedisConnectionProvider(new RedisConnectionConfiguration { Host = "localhost", Port = 6380 }));
/*
 * Only run the first time*/
//var dataLoader = services.BuildServiceProvider().GetService<IDataLoader>();
//await dataLoader!.LoadJsonAsync<RedisSearchLib.BaseProductDTO>(Constants.ProductKeyPrefix, "products.json");
//await dataLoader!.LoadJsonAsync<RedisSearchLib.TemplateDTO>(Constants.TemplateKeyPrefix, "templates.json");

//var omdataLoader = services.BuildServiceProvider().GetService<IOmDataLoader>();
//await omdataLoader!.LoadProductsJsonAsync("products.json");
//await omdataLoader!.LoadTemplatesJsonAsync("templates.json");
/**/

Console.WriteLine("============");
Console.WriteLine("NRedisStack:");
Console.WriteLine("============\r\n");

var indexer = services.BuildServiceProvider().GetService<IIndexer>();
var success = await indexer!.CreateProductIndexAsync(Constants.ProductIndexName, Constants.ProductKeyPrefix);
Console.WriteLine($"Product index created: {success}");
success = await indexer!.CreateTemplateIndexAsync(Constants.TemplateIndexName, Constants.TemplateKeyPrefix);
Console.WriteLine($"Template index created: {success}");

var searcher = services.BuildServiceProvider().GetService<ISearcher>();


await RunScenarioAsync(searcher!, "congratulations");
await RunScenarioAsync(searcher!, "banner");
await RunScenarioAsync(searcher!, "congratulations banner");
await RunScenarioAsync(searcher!, "frame");
await RunScenarioAsync(searcher!, "congratulations frame");

Console.WriteLine("=========");
Console.WriteLine("Redis.OM:");
Console.WriteLine("=========\r\n");

var idxCreationService = services.BuildServiceProvider().GetService<IIndexCreationService>();
await idxCreationService!.StartAsync();
var omSearcher = services.BuildServiceProvider().GetService<IOmSearcher>();

await RunOmScenarioAsync(omSearcher!, "congratulations");
await RunOmScenarioAsync(omSearcher!, "banner");
await RunOmScenarioAsync(omSearcher!, "congratulations banner");
await RunOmScenarioAsync(omSearcher!, "frame");
await RunOmScenarioAsync(omSearcher!, "congratulations frame");

Console.WriteLine("\r\n========================");
Console.WriteLine("Press any key to exit...");
Console.ReadLine();

static void DisplayResults(string keywords, NRedisStack.Search.SearchResult? products, NRedisStack.Search.SearchResult? templates)
{
    if (products != null)
    {
        Console.WriteLine($"Found {products.TotalResults} products matching '{keywords}'");
        if (products.Documents.Count > 0)
        {
            var results = products.ToJson();
            foreach (var doc in results)
            {
                var product = JsonConvert.DeserializeObject<RedisSearchLib.BaseProductDTO>(doc);
                Console.WriteLine($" - {product!.Name}");
            }
        }
    }
    else
    {
        Console.WriteLine($"No products found matching '{keywords}'");
    }

    if (templates != null)
    {
        Console.WriteLine($"Found {templates.TotalResults} templates matching '{keywords}'");
        if(templates.Documents.Count > 0)
        {
            var results = templates.ToJson();
            foreach (var doc in results)
            {
                var template = JsonConvert.DeserializeObject<RedisSearchLib.TemplateDTO>(doc);
                Console.WriteLine($" - {template!.Name}");
            }
        }
    }
    else
    {
        Console.WriteLine($"No templates found matching '{keywords}'");
    }
}

static void DisplayOmResults(string keywords, IList<RedisSearchOM.BaseProductDTO> products, IList<RedisSearchOM.TemplateDTO> templates)
{
    if (products != null)
    {
        Console.WriteLine($"Found {products.Count} products matching '{keywords}'");
        foreach (var p in products)
        {
            Console.WriteLine($" - {p.Name}");
        }
    }
    else
    {
        Console.WriteLine($"No products found matching '{keywords}'");
    }

    if (templates != null)
    {
        Console.WriteLine($"Found {templates.Count} templates matching '{keywords}'");
        foreach (var t in templates)
        {
            Console.WriteLine($" - {t.Name}");
        }
    }
    else
    {
        Console.WriteLine($"No templates found matching '{keywords}'");
    }
}

static async Task RunScenarioAsync(ISearcher searcher, string keywords)
{
    Console.WriteLine($"\r\n=== Searching '{keywords}' ===");
    var products = await searcher!.SearchProductsAsync(keywords);
    var templates = await searcher!.SearchTemplatesAsync(keywords);

    DisplayResults(keywords, products, templates);
}

static async Task RunOmScenarioAsync(IOmSearcher omSearcher, string keywords)
{
    Console.WriteLine($"\r\n=== Searching '{keywords}' ===");
    var omProducts = await omSearcher!.SearchProductsAsync(keywords);
    var omTemplates = await omSearcher!.SearchTemplatesAsync(keywords);

    DisplayOmResults(keywords, omProducts, omTemplates);
}