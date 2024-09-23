#### Why did you choose ASP.NET Core Web API over ASP.NET Core MVC?

Answer: I chose ASP.NET Core Web API because the task primarily focuses on fetching and returning data in a structured format (JSON) without requiring HTML rendering or front-end views. APIs are optimized for this purpose and provide a clean separation between front-end and back-end. If the goal was to serve a full website with views, I would have considered using ASP.NET Core MVC.

#### Why did you separate the services for CoinMarketCap and ExchangeRatesAPI into different service classes?

I followed the Single Responsibility Principle (SRP) by creating separate services for interacting with different APIs. Each service has a specific responsibility: CryptoService handles the cryptocurrency rates, and ExchangeRateService handles the currency exchange rates. This separation improves the maintainability and testability of the code, making each service easier to modify or replace independently without affecting the other.

#### Why did you choose HttpClient for making API calls?

HttpClient is the standard class in .NET for making HTTP requests. It's flexible, supports async calls, and integrates well with dependency injection in ASP.NET Core. Additionally, using HttpClientFactory helps manage connection pooling and reusability, avoiding socket exhaustion issues caused by creating too many HttpClient instances.

#### How would you handle API rate-limiting in this application?

 To handle API rate-limiting, I would:
- **Cache responses:** Use in-memory or distributed caching (like Redis) to store frequently requested exchange rates and cryptocurrency data for a set duration. This reduces the need for repeated API calls.
- **Retry logic:** Implement exponential backoff for retries if the API call is rate-limited or throttled.
- **Rate-limit monitoring:** Integrate monitoring and alerting (e.g., logging or using a service like Application Insights) to track API usage and ensure rate limits are respected.

#### How would you handle errors and exceptions in this application?

I would implement a centralized exception handling middleware to catch and log exceptions globally. This would prevent unhandled exceptions from crashing the application and provide a consistent response format for errors. 
For example:
- **API errors:** Ensure API calls handle errors gracefully by checking response status codes and providing fallback mechanisms (like retries or cached values).
- **Logging:** Log detailed errors with context using a structured logging framework such as Serilog or NLog.
- **Return standardized error responses:** Return a meaningful, user-friendly error message in the response body, along with an appropriate HTTP status code (e.g., 400 Bad Request, 500 Internal Server Error).

#### How would you ensure the security of this application, particularly regarding API keys?
To ensure security:

- **Secure API keys:** API keys should be stored securely in environment variables or secret management tools (e.g., Azure Key Vault or AWS Secrets Manager) instead of hardcoding them in the codebase.
- **HTTPS:** Ensure the application uses HTTPS for secure communication between the client and server, preventing man-in-the-middle attacks.
- **Rate limiting and authentication:** Implement rate limiting and possibly API authentication if this API will be consumed by external clients to protect against abuse.

#### How did you test your application?

I tested the application using both unit tests and integration tests:
- **Unit tests:** Focus on testing the individual services (CryptoService, ExchangeRateService) by mocking external dependencies like HTTP requests.
- **Integration tests:** Focus on testing the API endpoints as a whole to ensure the system behaves correctly when making real HTTP calls. Additionally, I used tools like Postman or Swagger to manually test the API endpoints and verify that they return the correct data.

#### What design patterns did you use in this project?

- **Dependency Injection:** The services (CryptoService and ExchangeRateService) were injected into the controller via the constructor. ASP.NET Core's built-in dependency injection helps manage the lifetime of services and decouples classes.
- **Factory pattern (via HttpClientFactory):** This pattern was used for efficient management of HttpClient instances, ensuring connections are reused, preventing issues like socket exhaustion.
- **Repository-like Pattern:** Each API interaction is encapsulated within a service class (CryptoService and ExchangeRateService), which acts similarly to a repository by abstracting away the external API logic.

#### How would you handle performance issues if the application becomes slow in production?

To address performance issues, I would:
- **Profile the application:** Use tools like Application Insights, DotTrace, or PerfView to identify bottlenecks in the application.
- **Database optimization (if applicable):** Ensure any data fetching is optimized and look for slow queries if a database is involved.
Caching: Introduce caching mechanisms for frequently requested data (e.g., cache crypto and exchange rates).
- **Optimize API calls:** Reduce the frequency of API calls by batching requests or adding a throttle to avoid unnecessary external requests.
- **Concurrency optimization:** Use asynchronous programming effectively to handle multiple requests without blocking threads.

#### What is the importance of async/await in your application, and how did you implement it?

The async/await pattern is crucial for improving scalability and performance by allowing non-blocking IO operations. In this application:
- API calls to CoinMarketCap and ExchangeRatesAPI are asynchronous, meaning the server can handle other requests while waiting for the external API responses.
- This prevents thread blocking, which can improve the overall throughput of the application, especially under high load.

#### How do you handle API versioning in this application?

API versioning is important for maintaining backward compatibility. While this project only contains one version, if future versions are needed, I would implement versioning by:
- URL versioning: Use versioned routes, such as /api/v1/crypto/{cryptoCode}.
- Query string versioning: Support a version parameter, e.g., /api/crypto?version=1.
- Header-based versioning: Implement versioning via custom headers, e.g., X-API-Version.
ASP.NET Core has built-in support for API versioning through the Microsoft.AspNetCore.Mvc.Versioning package.

#### How would you extend the application to support additional currencies or cryptocurrencies?

The application can easily be extended to support additional currencies or cryptocurrencies by:

- **Currencies:** Modify the controller to fetch rates for more currencies by adding them to the dictionary. The ExchangeRateService can handle any valid currency symbol provided by the ExchangeRatesAPI.
- **Cryptocurrencies:** The CryptoService is designed to handle any cryptocurrency supported by CoinMarketCap's API. To add support for more cryptocurrencies, the user can simply provide the crypto symbol (e.g., ETH, LTC) in the API request.

#### What could go wrong if you hardcode the base currency in ExchangeRateService?

Hardcoding the base currency reduces the flexibility of the application. If the base currency is fixed to USD, it becomes difficult to switch to another base currency (like EUR or GBP) without modifying the code. It also violates the Open/Closed Principle from SOLID principles, as the class must be changed whenever new base currencies are introduced. By making the base currency dynamic, the system can handle a wider range of use cases without changing the codebase.

#### What improvements could you make if you had more time?

- **Automated testing coverage:** Add more thorough unit and integration tests to cover edge cases and failures (e.g., handling API timeouts or invalid cryptocurrency codes).
Resilience: Implement circuit breakers and retry policies for API calls using libraries like Polly to handle transient faults in external APIs.
- **Caching:** Add caching for API responses to improve performance and reduce reliance on external API calls.
- **Pagination and filtering:** If the API grows, introduce pagination and filtering for large datasets.
- **Rate limiting and throttling:** Add rate-limiting to protect the API from abuse if deployed in a public environment.
- **Security:** Implement proper authentication and authorization if needed, such as API keys or OAuth.


#### What are the challenges of working with third-party APIs like CoinMarketCap and ExchangeRatesAPI?
Challenges of working with third-party APIs include:

- **Rate limits:** APIs often impose rate limits, which can cause issues in high-traffic applications if not managed properly.
- **Downtime or reliability:** External APIs can experience outages or be temporarily unavailable, which can impact your application unless handled correctly.
- **API changes:** APIs can change or deprecate certain features, requiring updates to the client application.
- **Latency:** Depending on the network, API calls can be slow, which may impact user experience unless you implement caching or async handling.











