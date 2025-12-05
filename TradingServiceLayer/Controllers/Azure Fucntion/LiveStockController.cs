namespace TradingServiceLayer.Controllers.Azure_Fucntion
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
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

               

               
                  var  entity = new LiveStockEntity
                    {
                        Symbol = liveData.Symbol
                    };
                    _db.LiveStock.Add(entity);
                

                entity.Price = liveData.Price;
                entity.CreatedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync();

                _logger.LogInformation($"📈 Live Update → {liveData.Symbol}: {liveData.Price}");

                var analytics = await _tradeService.GenerateSuggestionAsync(liveData);

                await _signalRNotifier.BroadcastAnalyticsAsync(analytics);
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


