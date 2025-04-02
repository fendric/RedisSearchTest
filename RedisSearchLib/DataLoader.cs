using StackExchange.Redis;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using Newtonsoft.Json;

namespace RedisSearchLib
{
    public interface IDataLoader
    {
        Task LoadJsonAsync<T>(string key, string filePath) where T : BaseDTO;
    }

    public class DataLoader : IDataLoader
    {
        private readonly IDatabase db;

        public DataLoader(IDatabase db)
        {
            this.db = db;
        }
        public async Task LoadJsonAsync<T>(string key, string filePath) where T : BaseDTO
        {

            JsonCommands cmd = db.JSON();
            //await cmd.SetFromFileAsync(key, "$", filePath);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File {filePath} not found.");
            }

            string fileContent = File.ReadAllText(filePath);

            var items = JsonConvert.DeserializeObject<IList<T>>(fileContent);
            if (items == null)
            {
                return;
            }
            foreach (var item in items)
            {
                await cmd.SetAsync($"{key}:{item.Id}", "$", item);
            }
        }
    }
}
