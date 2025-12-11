namespace LMS.API.Models.RequestModel
{
    public class FreePayment
    {
        public string payment_status { get; set; }
        public int payment_amount { get; set; }
        public string payment_currency { get; set; }
        public string payment_message { get; set; }
        public string payment_group { get; set; }

        // Optional fields (NOT present in JSON)
        public string? cf_payment_id { get; set; }
        public DateTime? payment_time { get; set; }
        public string? bank_reference { get; set; }
        public string? auth_id { get; set; }
        public FreePaymentMethod? payment_method { get; set; }
    }


    public class FreePaymentMethod
    {
        public FreeCardDetails? card { get; set; }
    }

    public class FreeCardDetails
    {
        public string? channel { get; set; }
        public string? card_number { get; set; }
        public string? card_network { get; set; }
        public string? card_type { get; set; }
        public string? card_sub_type { get; set; }
        public string? card_country { get; set; }
        public string? card_bank_name { get; set; }
    }

}
