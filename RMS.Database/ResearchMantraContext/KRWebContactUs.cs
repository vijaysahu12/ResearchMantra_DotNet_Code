using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRCRM.Database.KingResearchContext
{
    public class KRWebContactUs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long LeadId { get; set; }

        [Required]
        public string Remarks { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

    }
    public class LeadWithRemarksDto
    {
        public long LeadId { get; set; }
        public string FullName { get; set; }
        public string EmailId { get; set; }
        public string MobileNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ServiceKey { get; set; }
        public string LeadTypeKey { get; set; }
        public string LeadSourceKey { get; set; }
        public string AssignedTo { get; set; }
        public int StatusId { get; set; }
        public long KRWebContactUsId { get; set; }
        public string Remarks { get; set; }
        public DateTime ContactUsCreatedOn { get; set; }
    }


}
