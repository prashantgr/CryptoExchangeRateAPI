using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CryptoExchangeRateAPI.Models;

namespace CryptoExchangeRateAPI.Services
{
    public class CryptoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public CryptoService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["CoinMarketCapAPI:ApiKey"];
        }

        public virtual async Task<decimal> GetCryptoRateAsync(string cryptoCode)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://pro-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest?symbol={cryptoCode}&convert=USD"),
                Headers =
                {
                    { "X-CMC_PRO_API_KEY", _apiKey },
                },
            };

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<dynamic>(jsonString);

            return data.data[cryptoCode].quote.USD.price;
        }
    }
}