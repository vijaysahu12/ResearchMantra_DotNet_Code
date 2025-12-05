using TradingServiceLayer.Models.RequestModel;

namespace TradingServiceLayer.IServices
{
    public interface INotificationApi
    {
        Task<bool> BroadcastLiveUpdate(StockAnalyticsDto dto);
    }
}
