using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CryptoExchangeRateAPI.Models;

namespace CryptoExchangeRateAPI.Services
{
    public class ExchangeRateService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public ExchangeRateService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["ExchangeRatesAPI:ApiKey"];
        }

        public virtual async Task<ExchangeRateResponse> GetExchangeRatesAsync(string baseCurrency)
        {
            var response = await _httpClient.GetStringAsync($"https://api.exchangeratesapi.io/latest?access_key={_apiKey}&base={baseCurrency}");
            return JsonConvert.DeserializeObject<ExchangeRateResponse>(response);
        }
    }
}