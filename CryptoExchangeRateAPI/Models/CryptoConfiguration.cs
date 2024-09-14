namespace CryptoExchangeRateAPI.Models
{
    public record CryptoConfiguration
    {
        public string ExchangeRatesApiKey { get; init; }
        public string CoinMarketCapApiKey { get; init; }
    }
}
