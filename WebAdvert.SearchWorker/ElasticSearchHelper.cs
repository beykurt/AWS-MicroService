
using Microsoft.Extensions.Configuration;
using Nest;

namespace WebAdvert.SearchWorker
{
    public static class ElasticSearchHelper
    {
        private static IElasticClient _client;
        public static ElasticClient GetInstance(IConfiguration config)
        {
            if (_client == null)
            {
                var url = config.GetSection("ES").GetValue<string>("url");
                var settings = new ConnectionSettings(new Uri(url)).DefaultIndex("adverts").DefaultMappingFor<AdvertType>(m => m.IdProperty(x => x.Id));
                _client = new ElasticClient(settings);
            }
            return (ElasticClient)_client;
        }
    }
}
