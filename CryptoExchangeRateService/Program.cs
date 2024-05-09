using CryptoExchangeRateService.EnvironmentSettings;
using CryptoExchangeRateService.Helpers;
using CryptoExchangeRateService.Services;
using CryptoExchangeRateService.Services.CryptoMarkets;
using CryptoExchangeRateService.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Loading Environment Variables 
IConfigurationRoot config = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

config.GetRequiredSection("CryptoMarketUrls").Get<CryptoMarketUrls>();

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

builder.Services.AddTransient<IExchangeService, ExchangeService>();

// markets
builder.Services.AddTransient<ICryptoMarketService, KucoinService>();
builder.Services.AddTransient<ICryptoMarketService, BinanceService>();

// exchangeclient
builder.Services.AddTransient<ExchangeClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();