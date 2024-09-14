using CryptoExchangeRateAPI.Models;

namespace CryptoExchangeRateAPI.Interface
{
    public interface IExchangeRateService
    {
        Task<ExchangeRateResponse> GetExchangeRatesAsync(string baseCurrency);
    }
}
