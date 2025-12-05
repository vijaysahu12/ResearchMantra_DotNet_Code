using Microsoft.AspNetCore.Mvc;
using TradingServiceLayer.IServices;
using TradingServiceLayer.Models.RequestModel;

namespace TradingServiceLayer.Controllers.Notification
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly ISignalRNotifier _signalRNotifier;

        public NotificationController(
            ILogger<NotificationController> logger,
            ISignalRNotifier signalRNotifier)
        {
            _logger = logger;
            _signalRNotifier = signalRNotifier;
        }

        [HttpPost("broadcast/live-update")]
        public async Task<IActionResult> BroadcastLiveUpdate([FromBody] StockAnalyticsDto dto)
        {
            _logger.LogInformation($"📢 Broadcasting Update → {dto.Symbol} | {dto.CurrentPrice}");

            await _signalRNotifier.BroadcastAnalyticsAsync(dto);

            return Ok(new { Status = "Sent" });
        }
    }
}




public class LiveStockModel
{
    public string Symbol { get; set; }
    public decimal Price { get; set; }
}

