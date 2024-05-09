using CryptoExchangeRateService.Models;
using CryptoExchangeRateService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoExchangeRateService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExchangeController : ControllerBase
{
    private readonly IExchangeService _exchangeService;

    public ExchangeController(IExchangeService exchangeService)
    {
        _exchangeService = exchangeService;
    }

    [HttpGet("estimate")]
    public async Task<IActionResult> Estimate([FromQuery] EstimateModelRequest estimateModelRequest)
    {
        var bestOffer = await _exchangeService.Estimate(estimateModelRequest);

        if (bestOffer is null)
        {
            return BadRequest("Error occured or no markets found");
        }
        
        return Ok(bestOffer);
    }

    [HttpGet("get-rates")]
    public async Task<IActionResult> GetRates([FromQuery] GetRatesModelRequest getRates)
    {
        var rates = await _exchangeService.GetRates(getRates);

        if (rates is null || rates.All(x => x is null))
        {
            return BadRequest("Error occured");
        }
        
        return Ok(rates);
    }
}