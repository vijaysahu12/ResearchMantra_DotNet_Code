namespace LMS.API.Models.ResponseModel
{
    public class PaymentStatusRequestDTO
    {
        public string UserId { get; set; }       // GUID in string
        public string MerchantTransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }       // PENDING / SUCCESS / FAILED
        public List<int> ProductIds { get; set; }
    }


}
