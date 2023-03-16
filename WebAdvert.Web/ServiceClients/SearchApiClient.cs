using AutoMapper;
using System.Net;
using WebAdvert.Web.Models;

namespace WebAdvert.Web.ServiceClients
{
    public class SearchApiClient : ISearchApiClient
    {
        private readonly HttpClient _client;
        private readonly IMapper _mapper;
        private readonly string BaseAddress = string.Empty;
        public SearchApiClient(HttpClient client, IConfiguration configuration, IMapper mapper)
        {
            _mapper = mapper;
            _client = client;
            BaseAddress = configuration.GetSection("SearchApi").GetValue<string>("url");
        }

        public async Task<List<AdvertType>> Search(string keyword)
        {
            var result = new List<AdvertType>();
            var callUrl = $"{BaseAddress}/search/v1/{keyword}";
            var httpResponse = await _client.GetAsync(new Uri(callUrl)).ConfigureAwait(false);

            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var allAdvertsString = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var allAdverts = _mapper.Map<List<AdvertType>>(allAdvertsString);
                result.AddRange(allAdverts);
            }

            return result;
        }
    }
}
