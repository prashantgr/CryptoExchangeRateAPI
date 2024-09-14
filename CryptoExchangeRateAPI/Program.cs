using CryptoExchangeRateAPI.Interface;
using CryptoExchangeRateAPI.Models;
using CryptoExchangeRateAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register configuration
builder.Services.Configure<CryptoConfiguration>(builder.Configuration.GetSection("CryptoConfiguration"));

// Register HttpClient
builder.Services.AddHttpClient();

// Register interfaces and their implementations
builder.Services.AddScoped<ICryptoService, CryptoService>();
builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
