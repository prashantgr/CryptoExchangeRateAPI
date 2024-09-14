namespace CryptoExchangeRateAPI.Models
{
    public record CryptoQuote
    {
        public string Currency { get; init; }
        public decimal Rate { get; init; }
    }
}
