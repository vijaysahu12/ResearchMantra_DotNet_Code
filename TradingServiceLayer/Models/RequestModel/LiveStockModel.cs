namespace TradingServiceLayer.Models.RequestModel
{
    public class LiveStockModel
    {
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public decimal Change { get; set; }
        public long Volume { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
