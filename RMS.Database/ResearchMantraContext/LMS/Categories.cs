using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRCRM.Database.KingResearchContext.LMS
{
    public partial class Categories
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? GroupName { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsActive { get; set; }
    }
}
