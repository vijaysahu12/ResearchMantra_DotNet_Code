using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRCRM.Database.KingResearchContext
{
    public class EventPaymentDetails
    {
        public int PaymentId { get; set; }
        public int RegistrationId { get; set; }
        public decimal? PaymentAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentGateway { get; set; }
        public string TransactionId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? RefundAmount { get; set; }
        public DateTime? RefundDate { get; set; }
        public string? RefundReason { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public string? MerchantTransactionId { get; set; }

        //public EventRegistrationDetails Registration { get; set; }
    }

    //public class EventPaymentStatus
    //{
    //    public long Id { get; set; }
    //    public long UserId { get; set; }
    //    public long EventScheduleId { get; set; }
    //    public string? MerchantTransactionId { get; set; }
    //    public string? TransactionId { get; set; }
    //    public decimal Amount { get; set; }
    //    public string? PaymentMethod { get; set; }
    //    public DateTime? PaymentDate { get; set; }
    //    public DateTime CreatedOn { get; set; }
    //    public int CreatedBy { get; set; }
    //    public string PaymentStatus { get; set; }
    //}
}
