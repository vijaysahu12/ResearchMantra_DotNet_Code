using TradingServiceLayer.IServices;
using TradingServiceLayer.Models.RequestModel;

namespace TradingServiceLayer.Services
{
    public class NotificationApi : INotificationApi
    {
        private readonly HttpClient _client;
        private readonly ILogger<NotificationApi> _logger;

        public NotificationApi(HttpClient client, ILogger<NotificationApi> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<bool> BroadcastLiveUpdate(StockAnalyticsDto dto)
        {
            try
            {
                var response = await _client.PostAsJsonAsync("api/notification/broadcast/live-update", dto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to broadcast live update.");
                return false;
            }
        }
    }
}
