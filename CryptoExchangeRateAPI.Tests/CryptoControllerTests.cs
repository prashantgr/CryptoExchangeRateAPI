using Xunit;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CryptoExchangeRateAPI.Controllers;
using CryptoExchangeRateAPI.Services;
using System.Collections.Generic;
using System;
using CryptoExchangeRateAPI.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using CryptoExchangeRateAPI.Interface;

namespace CryptoExchangeRateAPI.Tests
{
    public class CryptoControllerTests
    {
        private readonly Mock<ICryptoService> _mockCryptoService;
        private readonly Mock<IExchangeRateService> _mockExchangeRateService;
        private readonly CryptoController _controller;

        public CryptoControllerTests()
        {
            var mockConfiguration = new Mock<IConfiguration>();
            _mockCryptoService = new Mock<ICryptoService>();
            _mockExchangeRateService = new Mock<IExchangeRateService>();
            mockConfiguration.SetupGet(c => c["CoinMarketCapAPI:ApiKey"]).Returns("fake-api-key");
            mockConfiguration.SetupGet(c => c["ExchangeRatesAPI:ApiKey"]).Returns("fake-api-key");
            _controller = new CryptoController(_mockCryptoService.Object, _mockExchangeRateService.Object);
        }

        [Fact]
        public async Task GetCryptoRates_ReturnsOkResult_WithRates()
        {
            // Arrange
            var cryptoCode = "BTC";
            var cryptoRateInUsd = new CryptoQuote { Rate = 50000m };
            var exchangeRates = new ExchangeRateResponse
            {
                Rates = new Dictionary<string, decimal>
                {
                    { "EUR", 0.85m },
                    { "BRL", 5.25m },
                    { "GBP", 0.75m },
                    { "AUD", 1.30m }
                }
            };

            _mockCryptoService.Setup(s => s.GetCryptoRateAsync(cryptoCode)).ReturnsAsync(cryptoRateInUsd);
            _mockExchangeRateService.Setup(s => s.GetExchangeRatesAsync("USD")).ReturnsAsync(exchangeRates);

            // Act
            var result = await _controller.GetCryptoRates(cryptoCode) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var rates = result.Value as Dictionary<string, decimal>;
            Assert.NotNull(rates);
            Assert.Equal(cryptoRateInUsd.Rate, rates["USD"]);
            Assert.Equal(cryptoRateInUsd.Rate * exchangeRates.Rates["EUR"], rates["EUR"]);
            Assert.Equal(cryptoRateInUsd.Rate * exchangeRates.Rates["BRL"], rates["BRL"]);
            Assert.Equal(cryptoRateInUsd.Rate * exchangeRates.Rates["GBP"], rates["GBP"]);
            Assert.Equal(cryptoRateInUsd.Rate * exchangeRates.Rates["AUD"], rates["AUD"]);
        }

        [Fact]
        public async Task GetCryptoRates_ReturnsBadRequest_OnException()
        {
            // Arrange
            var cryptoCode = "BTC";
            var exceptionMessage = "An error occurred";

            _mockCryptoService.Setup(s => s.GetCryptoRateAsync(cryptoCode)).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetCryptoRates(cryptoCode) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);

            var error = result.Value as Dictionary<string, string>;
            Assert.NotNull(error);
            Assert.Equal(exceptionMessage, error["error"]);
        }

        [Fact]
        public async Task GetCryptoRates_ReturnsBadRequest_OnCryptoServiceException()
        {
            // Arrange
            var cryptoCode = "BTC";
            var exceptionMessage = "Crypto service error";

            _mockCryptoService.Setup(s => s.GetCryptoRateAsync(cryptoCode)).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetCryptoRates(cryptoCode) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);

            var error = result.Value as Dictionary<string, string>;
            Assert.NotNull(error);
            Assert.Equal(exceptionMessage, error["error"]);
        }

        [Fact]
        public async Task GetCryptoRates_ReturnsBadRequest_OnExchangeRateServiceException()
        {
            // Arrange
            var cryptoCode = "BTC";
            var cryptoRateInUsd = new CryptoQuote { Rate = 50000m };
            var exceptionMessage = "Response status code does not indicate success: 401 (Unauthorized).";

            _mockCryptoService.Setup(s => s.GetCryptoRateAsync(cryptoCode)).ReturnsAsync(cryptoRateInUsd);
            _mockExchangeRateService.Setup(s => s.GetExchangeRatesAsync("USD")).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetCryptoRates(cryptoCode) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);

            var error = result.Value as Dictionary<string, string>;
            Assert.NotNull(error);
            Assert.Equal(exceptionMessage, error["error"]);
        }

        [Fact]
        public async Task GetCryptoRates_ReturnsBadRequest_OnInvalidCryptoCode()
        {
            // Arrange
            var cryptoCode = "INVALID";
            var exceptionMessage = "Cannot perform runtime binding on a null reference";

            _mockCryptoService.Setup(s => s.GetCryptoRateAsync(cryptoCode)).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetCryptoRates(cryptoCode) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);

            var error = result.Value as Dictionary<string, string>;
            Assert.NotNull(error);
            Assert.Equal(exceptionMessage, error["error"]);
        }
    }
}
