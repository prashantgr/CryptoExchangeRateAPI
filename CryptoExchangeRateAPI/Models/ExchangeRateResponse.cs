namespace CryptoExchangeRateAPI.Models
{
    public class ExchangeRateResponse
    {
        public Dictionary<string, decimal> Rates { get; set; }
        public string Base { get; set; }
        public DateTime Date { get; set; }
    }
}
