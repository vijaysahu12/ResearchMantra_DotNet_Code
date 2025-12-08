namespace TradingServiceLayer.Controllers.Azure_Fucntion
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Text.Json;
    using TradingServiceLayer.DbContextFolder;
    using TradingServiceLayer.Entity;
    using TradingServiceLayer.IServices;
    using TradingServiceLayer.Models.RequestModel;

    [Route("api/live")]
    [ApiController]
    public class LiveStockController : ControllerBase
    {
        private readonly DatabaseContext _db;
        private readonly ILogger<LiveStockController> _logger;
        private readonly ITradeService _tradeService;
        private readonly ISignalRNotifier _signalRNotifier;
        private readonly INotificationApi _notificationApi;

        public LiveStockController(
            DatabaseContext db,
            ILogger<LiveStockController> logger,
            ITradeService tradeService,
            ISignalRNotifier signalRNotifier,
            INotificationApi notificationApi)
        {
            _db = db;
            _logger = logger;
            _tradeService = tradeService;
            _signalRNotifier = signalRNotifier;
            _notificationApi = notificationApi;
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateLiveStock([FromBody] LiveStockModel liveData)
        {
            try
            {
                if (liveData == null)
                    return BadRequest("Invalid stock data");

               

               
                  var  entity = new LiveStockEntity
                    {
                        Symbol = liveData.Symbol
                    };
                    _db.LiveStock.Add(entity);
                

                entity.Price = liveData.Price;
                entity.CreatedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync();

                _logger.LogInformation($"📈 Live Update → {liveData.Symbol}: {liveData.Price}");
                Console.WriteLine("update AIp form live");

                //var analytics = await _tradeService.GenerateSuggestionAsync(liveData);
                var liveTradeData = await _db.LiveStock
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new StockAnalyticsDto
            {
                Symbol = x.Symbol,
                CurrentPrice = x.Price,
                BuyRecommendation = 0,
                Trend = "NA",
                UpdatedAt = x.CreatedAt
            })
            .ToListAsync();

                // wrap it inside ListOfStock
                var dto = new ListOfLatestStock
                {
                    stockData = liveTradeData
                };

                // Send full list to frontend
                await _signalRNotifier.BroadcastAnalyticsAsync(dto);
               
               // await _notificationApi.BroadcastLiveUpdate(analytics);

                return Ok(new
                {
                    Message = "Live stock updated successfully",
                    Data = dto
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing live stock update");
                return StatusCode(500, "Error processing update");
            }
        }


        [HttpPost("add-chat-message")]
        public async Task<IActionResult> AddChatMessage([FromBody] List<CommunityChatModel> chatList)
        {
            try
            {
                if (chatList == null || chatList.Count == 0)
                    return BadRequest("Invalid chat data");

                foreach (var chatData in chatList)
                {
                    var entity = new CommunityChatEntity
                    {
                        UserName = chatData.UserName,
                        Message = chatData.Message,
                        CreatedAt = DateTime.UtcNow
                    };

                    _db.CommunityChat.Add(entity);
                }

                await _db.SaveChangesAsync();

              

                var dto = new CommunityChatListDto
                {
                    Messages = chatList
                };

                // Broadcast to frontend
                await _signalRNotifier.BroadcastChatAsync(dto);

                return Ok(new
                {
                    Message = "Chat messages added",
                    Data = dto
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding chat messages");
                return StatusCode(500, "Error processing chat message");
            }
        }


    }
}


