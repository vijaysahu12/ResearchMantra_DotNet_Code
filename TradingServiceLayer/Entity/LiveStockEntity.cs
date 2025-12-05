namespace TradingServiceLayer.Entity
{
    public class LiveStockEntity
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
