using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRCRM.Database.KingResearchContext.LMS
{
    public class CashfreeWebhookTemp
    {
        [Key]
        public int Id { get; set; }
        public string RawData { get; set; }
       
            public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        

    }

}
