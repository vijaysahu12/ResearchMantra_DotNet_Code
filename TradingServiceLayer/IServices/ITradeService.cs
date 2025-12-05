using TradingServiceLayer.Models.RequestModel;

namespace TradingServiceLayer.IServices
{
    public interface ITradeService
    {
        Task<StockAnalyticsDto> GenerateSuggestionAsync(LiveStockModel data);
    }
}
