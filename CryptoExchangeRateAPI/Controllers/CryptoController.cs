using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CryptoExchangeRateAPI.Services;

namespace CryptoExchangeRateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CryptoController : ControllerBase
    {
        private readonly CryptoService _cryptoService;
        private readonly ExchangeRateService _exchangeRateService;

        public CryptoController(CryptoService cryptoService, ExchangeRateService exchangeRateService)
        {
            _cryptoService = cryptoService;
            _exchangeRateService = exchangeRateService;
        }

        [HttpGet("{cryptoCode}")]
        public async Task<IActionResult> GetCryptoRates(string cryptoCode)
        {
            try
            {
                var cryptoRateInUSD = await _cryptoService.GetCryptoRateAsync(cryptoCode);
                var exchangeRates = await _exchangeRateService.GetExchangeRatesAsync("USD");

                var rates = new Dictionary<string, decimal>
                {
                    { "USD", cryptoRateInUSD },
                    { "EUR", cryptoRateInUSD * exchangeRates.Rates["EUR"] },
                    { "BRL", cryptoRateInUSD * exchangeRates.Rates["BRL"] },
                    { "GBP", cryptoRateInUSD * exchangeRates.Rates["GBP"] },
                    { "AUD", cryptoRateInUSD * exchangeRates.Rates["AUD"] }
                };

                return Ok(rates);
            }
            catch (Exception ex)
            {
                // Add logging here
                Console.WriteLine($"Exception caught: {ex.Message}");
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}