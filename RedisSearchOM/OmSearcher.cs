using Redis.OM;
using Redis.OM.Searching;

namespace RedisSearchOM
{
    public interface IOmSearcher
    {
        Task<IList<BaseProductDTO>> SearchProductsAsync(string keywords);
        Task<IList<TemplateDTO>> SearchTemplatesAsync(string keywords);
    }

    public class OmSearcher : IOmSearcher
    {
        private readonly RedisConnectionProvider _provider;
        private readonly RedisCollection<BaseProductDTO> _products;
        private readonly RedisCollection<TemplateDTO> _templates;

        public OmSearcher(RedisConnectionProvider provider)
        {
            _provider = provider;
            _products = (RedisCollection<BaseProductDTO>)provider.RedisCollection<BaseProductDTO>();
            _templates = (RedisCollection<TemplateDTO>)provider.RedisCollection<TemplateDTO>();
        }

        public async Task<IList<BaseProductDTO>> SearchProductsAsync(string keywords)
        {
            if (!string.IsNullOrEmpty(keywords))
            {
                keywords = keywords.Replace(' ', '|');   //use an OR separator. Default is to use AND.
            }
            return await _products.Where(x => x.Published && !x.Deleted && 
                (
                    x.Name.Contains(keywords) || 
                    x.ShortDescription.Contains(keywords) || 
                    x.FullDescription.Contains(keywords) || 
                    x.Tags.Contains(keywords))
                ).ToListAsync();
        }

        public async Task<IList<TemplateDTO>> SearchTemplatesAsync(string keywords)
        {
            if (!string.IsNullOrEmpty(keywords))
            {
                keywords = keywords.Replace(' ', '|');   //use an OR separator. Default is to use AND.
            }
            return await _templates.Where(x => x.Published && !x.Deleted && (x.Name.Contains(keywords) || x.Tags.Contains(keywords))).ToListAsync();
        }
    }
}
