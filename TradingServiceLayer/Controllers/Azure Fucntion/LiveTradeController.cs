using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Azure.Messaging.ServiceBus;
using TradingServiceLayer.Models.RequestModel;
using TradingServiceLayer.IServices;

namespace TradingServiceLayer.Controllers.Azure_Fucntion
{
    
    public class LiveTradeController 
    {
            private readonly ILogger<LiveTradeController> _logger;
            private readonly ITradeService _tradeService;
            private readonly INotificationApi _notificationApi;
            private readonly ISignalRNotifier _signalRNotifier;


        public LiveTradeController(
                ILogger<LiveTradeController> logger,
                ITradeService tradeService,
                INotificationApi notificationApi, ISignalRNotifier signalRNotifier)
            {
                _logger = logger;
                _tradeService = tradeService;
                _notificationApi = notificationApi;
                _signalRNotifier = signalRNotifier;
            }

            [Function("LiveStockUpdateFunction")]
            public async Task Run(
                [ServiceBusTrigger("rm-stock-live-updates", Connection = "SERVICEBUS_CONNECTION")]
            ServiceBusReceivedMessage message,
                ServiceBusMessageActions messageActions)
            {
                string messageId = message.MessageId;

                try
                {
                    // 1. Deserialize
                    var body = Encoding.UTF8.GetString(message.Body);
                    var liveData = JsonSerializer.Deserialize<LiveStockModel>(
                        body,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (liveData == null)
                    {
                        _logger.LogError($"Message {messageId} failed to deserialize.");
                        return;
                    }

                    _logger.LogInformation($" Live Update → {liveData.Symbol}: {liveData.Price}");

                    // 2. Analytics calculation
                    var analytics = await _tradeService.GenerateSuggestionAsync(liveData);

                    // 3. Notify all users via NotificationService
                     await _signalRNotifier.BroadcastAnalyticsAsync(analytics);
                    await _notificationApi.BroadcastLiveUpdate(analytics);
                // 4. Complete the message
                await messageActions.CompleteMessageAsync(message);

                    _logger.LogInformation("✅ Live update processed successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"🔥 Error processing live stock update: {messageId}");
                    throw; // triggers retry
                }
            }
        }
}

