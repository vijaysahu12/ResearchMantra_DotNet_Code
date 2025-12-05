namespace TradingServiceLayer.Controllers.Azure_Fucntion
{
    using Microsoft.AspNetCore.Mvc;
    using System.Text.Json;
    using TradingServiceLayer.DbContextFolder;
    using TradingServiceLayer.Entity;
    using TradingServiceLayer.IServices;

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

                // 1. Save to DB
                var entity = new LiveStockEntity
                {
                    Symbol = liveData.Symbol,
                    Price = liveData.Price
                };

                await _db.LiveStocks.AddAsync(entity);
                await _db.SaveChangesAsync();

                _logger.LogInformation($"📈 Live Update → {liveData.Symbol}: {liveData.Price}");

                // 2. Generate analytics
                var analytics = await _tradeService.GenerateSuggestionAsync(liveData);

                // 3. SignalR broadcast
                await _signalRNotifier.BroadcastAnalyticsAsync(analytics);

                // 4. Notify all user devices via Notification API
                await _notificationApi.BroadcastLiveUpdate(analytics);

                return Ok(new
                {
                    Message = "Live stock updated successfully",
                    Data = analytics
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing live stock update");
                return StatusCode(500, "Error processing update");
            }
        }
    }
}


