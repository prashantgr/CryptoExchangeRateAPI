using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CryptoExchangeRateAPI.Services;
using CryptoExchangeRateAPI.Interface;

namespace CryptoExchangeRateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CryptoController : ControllerBase
    {
        private readonly ICryptoService _cryptoService;
        private readonly IExchangeRateService _exchangeRateService;

        public CryptoController(ICryptoService cryptoService, IExchangeRateService exchangeRateService)
        {
            _cryptoService = cryptoService;
            _exchangeRateService = exchangeRateService;
        }

        [HttpGet("{cryptoCode}")]
        public async Task<IActionResult> GetCryptoRates(string cryptoCode)
        {
            try
            {
                // Get the rate of the cryptocurrency in USD
                var cryptoRateInUsd = await _cryptoService.GetCryptoRateAsync(cryptoCode);

                // Get the exchange rates for USD
                var exchangeRates = await _exchangeRateService.GetExchangeRatesAsync("USD");

                // Calculate the rates for other currencies
                var rates = new Dictionary<string, decimal>
                {
                    { "USD", cryptoRateInUsd.Rate }, // Access the Rate property
                    { "EUR", cryptoRateInUsd.Rate * exchangeRates.Rates["EUR"] },
                    { "BRL", cryptoRateInUsd.Rate * exchangeRates.Rates["BRL"] },
                    { "GBP", cryptoRateInUsd.Rate * exchangeRates.Rates["GBP"] },
                    { "AUD", cryptoRateInUsd.Rate * exchangeRates.Rates["AUD"] }
                };

                return Ok(rates);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");
                var errorResponse = new Dictionary<string, string>
                {
                    { "error", ex.Message }
                };
                return BadRequest(errorResponse);
            }
        }
    }
}