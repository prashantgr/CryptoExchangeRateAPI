using System.Net.Http;
using System.Threading.Tasks;
using CryptoExchangeRateAPI.Interface;
using Newtonsoft.Json;
using CryptoExchangeRateAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CryptoExchangeRateAPI.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly CryptoConfiguration _cryptoConfiguration;

        public ExchangeRateService(HttpClient httpClient, IOptions<CryptoConfiguration> appConfiguration)
        {
            _httpClient = httpClient;
            _cryptoConfiguration = appConfiguration.Value;
        }

        // Get the exchange rates for the specified base currency
        public async Task<ExchangeRateResponse?> GetExchangeRatesAsync(string baseCurrency)
        {
            
            var response = await _httpClient.GetStringAsync($"https://api.exchangeratesapi.io/latest?access_key={_cryptoConfiguration.ExchangeRatesApiKey}&base={baseCurrency}");
            return JsonConvert.DeserializeObject<ExchangeRateResponse>(response);
        }
    }
}