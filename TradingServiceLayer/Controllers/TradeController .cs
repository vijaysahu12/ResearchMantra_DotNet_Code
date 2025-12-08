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

        [HttpGet("get-all-chat")]
        public async Task<IActionResult> GetAllChatMessages()
        {
            try
            {
                var messages = await _db.CommunityChat
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(x => new CommunityChatModel
                    {
                        UserName = x.UserName,
                        Message = x.Message,
                        CreatedAt = x.CreatedAt
                    })
                    .ToListAsync();

                var dto = new CommunityChatListDto
                {
                    Messages = messages
                };

                return Ok(new
                {
                    Message = "All chat messages fetched successfully",
                    Data = dto
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching chat messages");
                return StatusCode(500, "Error fetching chat messages");
            }
        }


    }
}
