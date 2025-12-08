using TradingServiceLayer.Models.RequestModel;

namespace TradingServiceLayer.IServices
{
    public interface ISignalRNotifier
    {
        Task BroadcastAnalyticsAsync(ListOfLatestStock dto);
        Task BroadcastChatAsync(CommunityChatListDto dto);
    }
}
