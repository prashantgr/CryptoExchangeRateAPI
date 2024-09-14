using CryptoExchangeRateAPI.Models;

namespace CryptoExchangeRateAPI.Interface
{
    public interface ICryptoService
    {
        Task<CryptoQuote> GetCryptoRateAsync(string cryptoCode);
    }
}
