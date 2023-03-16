using AdvertApi.Models;
using AutoMapper;
using Newtonsoft.Json;
using System.Text;

namespace WebAdvert.Web.ServiceClients
{
    public class AdvertApiClient : IAdvertApiClient
    {
        private readonly HttpClient _client;
        private readonly IMapper _mapper;
        private readonly string _baseAddress;

        public AdvertApiClient(IConfiguration configuration, HttpClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
            _baseAddress = configuration.GetSection("AdvertApi").GetValue<string>("BaseUrl");
        }

        public async Task<AdvertResponse> CreateAsync(CreateAdvertModel model)
        {
            var advertApiModel = _mapper.Map<AdvertModel>(model);
            var jsonModel = JsonConvert.SerializeObject(advertApiModel);
            var response = await _client.PostAsync(new Uri($"{_baseAddress}/create"), new StringContent(jsonModel, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var createAdvertResponse = JsonConvert.DeserializeObject<CreateAdvertResponse>(responseJson);
            var advertResponse = _mapper.Map<AdvertResponse>(createAdvertResponse);
            return advertResponse;
        }

        public async Task<bool> ConfirmAsync(ConfirmAdvertRequest model)
        {
            var advertModel = _mapper.Map<ConfirmAdvertRequest>(model);
            var jsonModel = JsonConvert.SerializeObject(advertModel);
            var response = await _client.PutAsync(new Uri($"{_baseAddress}/confirm"), new StringContent(jsonModel, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }
        public async Task<List<Advertisement>> GetAllAsync()
        {
            var apiCallResponse = await _client.GetAsync(new Uri($"{_baseAddress}/all")).ConfigureAwait(false);
            var allAdvertModelsString = await apiCallResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var allAdvertModels = JsonConvert.DeserializeObject<List<Advertisement>>(allAdvertModelsString);
            return allAdvertModels;
        }

        public async Task<Advertisement> GetAsync(string advertId)
        {
            var apiCallResponse = await _client.GetAsync(new Uri($"{_baseAddress}/{advertId}")).ConfigureAwait(false);
            var fullAdvertString = await apiCallResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var fullAdvert = JsonConvert.DeserializeObject<Advertisement>(fullAdvertString);
            return fullAdvert;
        }
    }
}
