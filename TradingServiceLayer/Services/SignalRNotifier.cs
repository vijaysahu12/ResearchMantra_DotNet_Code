using Microsoft.AspNetCore.SignalR;
using TradingServiceLayer.Hubs;
using TradingServiceLayer.IServices;
using TradingServiceLayer.Models.RequestModel;

namespace TradingServiceLayer.Services
{
    public class SignalRNotifier : ISignalRNotifier
    {
        private readonly IHubContext<LiveTradeHub> _hub;

        public SignalRNotifier(IHubContext<LiveTradeHub> hub)
        {
            _hub = hub;
        }

        public async Task BroadcastAnalyticsAsync(StockAnalyticsDto dto)
        {
            // Send message to all connected WebSocket clients
            await _hub.Clients.All.SendAsync("LiveStockUpdate", dto);
        }
    }
}
