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

namespace CryptoExchangeRateAPI.Tests
{
    public class CryptoControllerTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<HttpClient> _mockHttpClient;
        private readonly CryptoService _cryptoService;
        private readonly ExchangeRateService _exchangeRateService;
        private readonly CryptoController _controller;

        public CryptoControllerTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockHttpClient = new Mock<HttpClient>();

            _mockConfiguration.SetupGet(c => c["CoinMarketCapAPI:ApiKey"]).Returns("fake-api-key");
            _mockConfiguration.SetupGet(c => c["ExchangeRatesAPI:ApiKey"]).Returns("fake-api-key");

            _cryptoService = new CryptoService(_mockHttpClient.Object, _mockConfiguration.Object);
            _exchangeRateService = new ExchangeRateService(_mockHttpClient.Object, _mockConfiguration.Object);
            _controller = new CryptoController(_cryptoService, _exchangeRateService);
        }

        [Fact]
        public async Task GetCryptoRates_ReturnsOkResult_WithRates()
        {
            // Arrange
            var cryptoCode = "BTC";
            var cryptoRateInUSD = 50000m;
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

            var mockCryptoService = new Mock<CryptoService>(_mockHttpClient.Object, _mockConfiguration.Object);
            var mockExchangeRateService = new Mock<ExchangeRateService>(_mockHttpClient.Object, _mockConfiguration.Object);

            mockCryptoService.Setup(s => s.GetCryptoRateAsync(cryptoCode)).ReturnsAsync(cryptoRateInUSD);
            mockExchangeRateService.Setup(s => s.GetExchangeRatesAsync("USD")).ReturnsAsync(exchangeRates);

            var controller = new CryptoController(mockCryptoService.Object, mockExchangeRateService.Object);

            // Act
            var result = await controller.GetCryptoRates(cryptoCode) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var rates = result.Value as Dictionary<string, decimal>;
            Assert.NotNull(rates);
            Assert.Equal(cryptoRateInUSD, rates["USD"]);
            Assert.Equal(cryptoRateInUSD * exchangeRates.Rates["EUR"], rates["EUR"]);
            Assert.Equal(cryptoRateInUSD * exchangeRates.Rates["BRL"], rates["BRL"]);
            Assert.Equal(cryptoRateInUSD * exchangeRates.Rates["GBP"], rates["GBP"]);
            Assert.Equal(cryptoRateInUSD * exchangeRates.Rates["AUD"], rates["AUD"]);
        }

        [Fact]
        public async Task GetCryptoRates_ReturnsBadRequest_OnException()
        {
            // Arrange
            var cryptoCode = "BTC";
            var exceptionMessage = "An error occurred";

            var mockCryptoService = new Mock<CryptoService>(_mockHttpClient.Object, _mockConfiguration.Object);
            var mockExchangeRateService = new Mock<ExchangeRateService>(_mockHttpClient.Object, _mockConfiguration.Object);

            mockCryptoService.Setup(s => s.GetCryptoRateAsync(cryptoCode)).ThrowsAsync(new Exception(exceptionMessage));

            var controller = new CryptoController(mockCryptoService.Object, mockExchangeRateService.Object);

            // Act
            var result = await controller.GetCryptoRates(cryptoCode) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);

            // Check if result.Value is a dictionary
            if (result.Value is IDictionary<string, object> errorDict)
            {
                Assert.Equal(exceptionMessage, errorDict["error"]);
            }
            // Check if result.Value is an anonymous object
            else if (result.Value is { } errorObj)
            {
                var errorProperty = errorObj.GetType().GetProperty("error");
                Assert.NotNull(errorProperty);
                Assert.Equal(exceptionMessage, errorProperty.GetValue(errorObj));
            }
            else
            {
                Assert.True(false, "Unexpected error type");
            }
        }

        [Fact]
        public async Task GetCryptoRates_ReturnsOk_OnSuccess()
        {
            // Arrange
            var cryptoCode = "BTC";
            var cryptoRateInUSD = 50000m;
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

            var mockCryptoService = new Mock<CryptoService>(_mockHttpClient.Object, _mockConfiguration.Object);
            var mockExchangeRateService = new Mock<ExchangeRateService>(_mockHttpClient.Object, _mockConfiguration.Object);

            mockCryptoService.Setup(s => s.GetCryptoRateAsync(cryptoCode)).ReturnsAsync(cryptoRateInUSD);
            mockExchangeRateService.Setup(s => s.GetExchangeRatesAsync("USD")).ReturnsAsync(exchangeRates);
            var controller = new CryptoController(mockCryptoService.Object, mockExchangeRateService.Object);

            // Act
            var result = await controller.GetCryptoRates(cryptoCode) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var rates = result.Value as Dictionary<string, decimal>;
            Assert.NotNull(rates);
            Assert.Equal(cryptoRateInUSD, rates["USD"]);
            Assert.Equal(cryptoRateInUSD * exchangeRates.Rates["EUR"], rates["EUR"]);
            Assert.Equal(cryptoRateInUSD * exchangeRates.Rates["BRL"], rates["BRL"]);
            Assert.Equal(cryptoRateInUSD * exchangeRates.Rates["GBP"], rates["GBP"]);
            Assert.Equal(cryptoRateInUSD * exchangeRates.Rates["AUD"], rates["AUD"]);
        }

        [Fact]
        public async Task GetCryptoRates_ReturnsBadRequest_OnCryptoServiceException()
        {
            // Arrange
            var cryptoCode = "BTC";
            var exceptionMessage = "Crypto service error";

            var mockCryptoService = new Mock<CryptoService>(_mockHttpClient.Object, _mockConfiguration.Object);
            mockCryptoService.Setup(s => s.GetCryptoRateAsync(cryptoCode)).ThrowsAsync(new Exception(exceptionMessage));

            var controller = new CryptoController(mockCryptoService.Object, _exchangeRateService);

            // Act
            var result = await controller.GetCryptoRates(cryptoCode) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);

            var error = result.Value;
            Assert.NotNull(error);
        }

        [Fact]
        public async Task GetCryptoRates_ReturnsBadRequest_OnExchangeRateServiceException()
        {
            // Arrange
            var cryptoCode = "BTC";
            var cryptoRateInUSD = 50000m;
            var exceptionMessage = "Response status code does not indicate success: 401 (Unauthorized).";

            var mockCryptoService = new Mock<CryptoService>(_mockHttpClient.Object, _mockConfiguration.Object);
            var mockExchangeRateService = new Mock<ExchangeRateService>(_mockHttpClient.Object, _mockConfiguration.Object);
            mockCryptoService.Setup(s => s.GetCryptoRateAsync(cryptoCode)).ReturnsAsync(cryptoRateInUSD);
            mockExchangeRateService.Setup(s => s.GetExchangeRatesAsync("USD")).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetCryptoRates(cryptoCode) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);

            var error = result.Value;
            Assert.NotNull(error);
        }

        [Fact]
        public async Task GetCryptoRates_ReturnsBadRequest_OnInvalidCryptoCode()
        {
            // Arrange
            var cryptoCode = "INVALID";
            var exceptionMessage = "Invalid crypto code";
            var mockCryptoService = new Mock<CryptoService>(_mockHttpClient.Object, _mockConfiguration.Object);
            mockCryptoService.Setup(s => s.GetCryptoRateAsync(cryptoCode)).ThrowsAsync(new Exception(exceptionMessage));

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
