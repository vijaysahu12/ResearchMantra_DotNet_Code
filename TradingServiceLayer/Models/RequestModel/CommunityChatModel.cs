namespace TradingServiceLayer.Models.RequestModel
{
    public class CommunityChatModel
    {
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    }

    public class CommunityChatListDto
    {
        public IList<CommunityChatModel> Messages { get; set; }
    }

    public class CommunityChatMessageDto
    {
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
