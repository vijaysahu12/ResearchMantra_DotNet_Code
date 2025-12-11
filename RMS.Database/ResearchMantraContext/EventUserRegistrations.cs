using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRCRM.Database.KingResearchContext
{
    public class EventUserRegistrations
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long EventScheduleId { get; set; }
        public string EventCode { get; set; }
        public string? MerchantTransactionId { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
