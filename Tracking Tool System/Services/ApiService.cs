using System.Net.Http;
using System.Net.Http.Json;

namespace Tracking_Tool_System.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public ApiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;

            _httpClient.BaseAddress = new Uri(_config["ApiSettings:BaseUrl"]);
        }

        // GET
        public async Task<List<T>> GetAsync<T>(string endpoint)
        {
            return await _httpClient.GetFromJsonAsync<List<T>>(endpoint);
        }

        // POST
        public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T data)
        {
            return await _httpClient.PostAsJsonAsync(endpoint, data);
        }

        // PUT
        public async Task<HttpResponseMessage> PutAsync<T>(string endpoint, T data)
        {
            return await _httpClient.PutAsJsonAsync(endpoint, data);
        }

        // GET Single
        public async Task<T?> GetSingleAsync<T>(string endpoint)
        {
            return await _httpClient.GetFromJsonAsync<T>(endpoint);
        }
    }
}