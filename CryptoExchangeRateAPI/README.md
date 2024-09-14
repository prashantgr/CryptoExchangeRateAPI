# Crypto Exchange Rate API

This project is a simple API that provides cryptocurrency exchange rates using data from CoinMarketCap and ExchangeRatesAPI.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or any other IDE that supports .NET 8
- [Postman](https://www.postman.com/downloads/) (optional, for testing the API)
- [Swagger](https://swagger.io/) (for API documentation and testing)

## Getting Started

### Clone the Repository
git clone https://github.com/knabnl-incubator/Prashant-Rathod-Knab/CryptoExchangeRateAPI.git cd CryptoExchangeRateAPI


### Configuration

The application uses API keys from CoinMarketCap and ExchangeRatesAPI. These keys are stored in the `appsettings.json` file.

1. Open the `appsettings.json` file located in the root of the project.
2. Replace the placeholder API keys with your actual API keys.


### Build and Run the Application

1. Open the solution in Visual Studio.
2. Restore the NuGet packages.
3. Build the solution.
4. Run the application.



The API will be available at `http://localhost:5171/swagger/index.html`.

### Running Tests

To run the unit tests, you can use the Test Explorer in Visual Studio or run the following command:
- dotnet test


## API Endpoints

### Get Cryptocurrency Rates

- **URL:** `/api/crypto/{cryptoCode}`
- **Method:** `GET`
- **Description:** Returns the exchange rates for the specified cryptocurrency code.

#### Example Request
- **GET**  `/api/crypto/BTC`


#### Example Response

{ "USD": 50000, "EUR": 42500, "BRL": 262500, "GBP": 37500, "AUD": 65000 }




