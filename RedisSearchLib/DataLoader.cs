using StackExchange.Redis;
using NRedisStack;
using NRedisStack.RedisStackCommands;

namespace RedisSearchLib
{
    public interface IDataLoader
    {
        Task LoadJsonAsync(string key, string filePath);
    }

    public class DataLoader : IDataLoader
    {
        private readonly IDatabase db;

        public DataLoader(IDatabase db)
        {
            this.db = db;
        }
        public async Task LoadJsonAsync(string key, string filePath)
        {

            JsonCommands cmd = db.JSON();
            await cmd.SetFromFileAsync(key, "$", filePath);
        }
    }
}
