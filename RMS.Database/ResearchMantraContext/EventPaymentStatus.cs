using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRCRM.Database.KingResearchContext
{
    public class EventPaymentStatus
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long EventScheduleId { get; set; }
        public string MerchantTransactionId { get; set; }
        public string? TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public string? PaymentStatus { get; set; }
    }
}
