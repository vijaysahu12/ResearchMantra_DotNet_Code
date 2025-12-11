using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRCRM.Database.KingResearchContext
{
    public class Events
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal? Amount { get; set; }
        public string? ServiceId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
