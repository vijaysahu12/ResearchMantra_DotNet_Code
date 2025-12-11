namespace LMS.API.Models.RequestModel
{
    public class FreePaymentGatewayDetails
    {
        public string gateway_name { get; set; } = "FREE_PAYMENT";
        public string? gateway_order_id { get; set; } = null;
        public string? gateway_payment_id { get; set; } = null;
        public string? gateway_status_code { get; set; } = null;
        public string? gateway_order_reference_id { get; set; } = null;
        public string? gateway_settlement { get; set; } = "FREE";
        public string? gateway_reference_name { get; set; } = null;
    }

}
