namespace LMS.API.Models.RequestModel
{
    public class FreeOrderWebhookModel
    {
        public DateTime event_time { get; set; }
        public string type { get; set; }
        public FreeOrderData data { get; set; }
    }

    public class FreeOrderData
    {
        public FreeOrder order { get; set; }
        public FreePayment payment { get; set; }
        public FreeCustomerDetails customer_details { get; set; }
        public FreePaymentGatewayDetails payment_gateway_details { get; set; }
    }


}
