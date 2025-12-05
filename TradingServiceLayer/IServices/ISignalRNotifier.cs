using TradingServiceLayer.Models.RequestModel;

namespace TradingServiceLayer.IServices
{
    public interface ISignalRNotifier
    {
        Task BroadcastAnalyticsAsync(StockAnalyticsDto dto);
    }
}
