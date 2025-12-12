namespace ProductService.Models.RequestModel
{
    public class GetCouponsRequestModel
    {
        public int ProductId { get; set; }
        public int SubscriptionDurationId { get; set; }
    }
}
