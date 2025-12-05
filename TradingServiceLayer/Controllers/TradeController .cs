using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradingServiceLayer.DbContextFolder;
using TradingServiceLayer.Models.RequestModel;

namespace TradingServiceLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private readonly ILogger<TradeController> _logger;
        private readonly DatabaseContext _db;

        public TradeController(ILogger<TradeController> logger, DatabaseContext db)
        {
            _logger = logger;
            _db = db;
        }

        // GET: api/trade/live-stocks
        [HttpGet("live-stocks")]
        public async Task<IActionResult> GetLiveStocks()
        {
            _logger.LogInformation("Fetching latest live stock list from database...");

            var stocks = await _db.LiveStock
                .OrderByDescending(x => x.CreatedAt) // Optional
                .Select(x => new LiveStockModel
                {
                    Symbol = x.Symbol,
                    Price = x.Price
                })
                .ToListAsync();

            return Ok(stocks);
        }

    }
}
