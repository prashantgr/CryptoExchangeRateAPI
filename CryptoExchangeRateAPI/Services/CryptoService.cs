using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CryptoExchangeRateAPI.Models;
using CryptoExchangeRateAPI.Interface;
using Microsoft.Extensions.Options;

namespace CryptoExchangeRateAPI.Services
{
    public class CryptoService : ICryptoService
    {
        private readonly HttpClient _httpClient;
        private readonly CryptoConfiguration _cryptoConfiguration;

        public CryptoService(HttpClient httpClient, IOptions<CryptoConfiguration> appConfiguration)
        {
            _httpClient = httpClient;
            _cryptoConfiguration = appConfiguration.Value;
        }

        // Get the rate of the specified cryptocurrency in USD
        public async Task<CryptoQuote> GetCryptoRateAsync(string cryptoCode)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://pro-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest?symbol={cryptoCode}&convert=USD"),
                Headers =
                {
                    { "X-CMC_PRO_API_KEY", _cryptoConfiguration.CoinMarketCapApiKey },
                },
            };

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<dynamic>(jsonString);

            return new CryptoQuote
            {
                Currency = cryptoCode,
                Rate = data.data[cryptoCode].quote.USD.price
            };
        }
    }
}