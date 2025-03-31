using Redis.OM;

namespace RedisSearchOM
{
    public interface IIndexCreationService
    {
        Task StartAsync();
    }

    public class IndexCreationService : IIndexCreationService
    {
        private readonly RedisConnectionProvider _provider;

        public IndexCreationService(RedisConnectionProvider provider)
        {
            _provider = provider;
        }

        public async Task StartAsync()
        {
            await _provider.Connection.CreateIndexAsync(typeof(BaseProductDTO));
            await _provider.Connection.CreateIndexAsync(typeof(TemplateDTO));
        }
    }
}
