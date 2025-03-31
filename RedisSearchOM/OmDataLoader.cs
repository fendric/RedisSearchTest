using Newtonsoft.Json;
using Redis.OM;
using Redis.OM.Searching;

namespace RedisSearchOM
{
    public interface IOmDataLoader
    {
        Task LoadTemplatesJsonAsync(string filePath);
        Task LoadProductsJsonAsync(string filePath);
    }

    public class OmDataLoader : IOmDataLoader
    {
        private readonly RedisConnectionProvider _provider;
        private readonly RedisCollection<BaseProductDTO> _products;
        private readonly RedisCollection<TemplateDTO> _templates;

        public OmDataLoader(RedisConnectionProvider provider)
        {
            _provider = provider;
            _products = (RedisCollection<BaseProductDTO>)provider.RedisCollection<BaseProductDTO>();
            _templates = (RedisCollection<TemplateDTO>)provider.RedisCollection<TemplateDTO>();
        }

        public async Task LoadProductsJsonAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File " + filePath + " not found.");
            }

            string text = File.ReadAllText(filePath);

            var products = JsonConvert.DeserializeObject<List<BaseProductDTO>>(text);
            if (products == null)
            {
                throw new Exception("No products found in file.");
            }
            await _products.InsertAsync(products);
        }

        public async Task LoadTemplatesJsonAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File " + filePath + " not found.");
            }

            string text = File.ReadAllText(filePath);

            var templates = JsonConvert.DeserializeObject<List<TemplateDTO>>(text);
            if (templates == null)
            {
                throw new Exception("No templates found in file.");
            }
            await _templates.InsertAsync(templates);
        }
    }
}
