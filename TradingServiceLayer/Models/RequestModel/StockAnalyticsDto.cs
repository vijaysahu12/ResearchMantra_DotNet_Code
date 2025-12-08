namespace TradingServiceLayer.Models.RequestModel
{
    public class ListOfLatestStock
    {
        public IList<StockAnalyticsDto>? stockData { get; set; }
    }

    public class StockAnalyticsDto
    {
        public string Symbol { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal BuyRecommendation { get; set; }
        public string Trend { get; set; } // e.g., "Bullish", "Bearish"
        public DateTime UpdatedAt { get; set; }
    }
}
