namespace CryptoExchangeRateAPI.Models
{
    public record ExchangeRateResponse
    {
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
