using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRCRM.Database.KingResearchContext
{
    public class EventSchedule
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public string EventCode { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Location { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
