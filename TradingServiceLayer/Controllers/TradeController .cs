using Microsoft.AspNetCore.Mvc;
using TradingServiceLayer.Models.RequestModel;

namespace TradingServiceLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private readonly ILogger<TradeController> _logger;

        public TradeController(ILogger<TradeController> logger)
        {
            _logger = logger;
        }

        // GET: api/trade/live-stocks
        [HttpGet("live-stocks")]
        public IActionResult GetLiveStocks()
        {
            _logger.LogInformation("Fetching initial live stock list...");

            // Static sample data
            var stocks = new List<LiveStockModel>
            {
                new LiveStockModel { Symbol = "AAPL", Price = 191.25m },
                new LiveStockModel { Symbol = "GOOGL", Price = 2821.50m },
                new LiveStockModel { Symbol = "MSFT", Price = 375.60m },
                new LiveStockModel { Symbol = "TSLA", Price = 251.33m },
                new LiveStockModel { Symbol = "AMZN", Price = 145.18m }
            };

            return Ok(stocks);
        }
    }
}
