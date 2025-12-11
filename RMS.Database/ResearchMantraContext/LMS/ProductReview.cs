using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRCRM.Database.KingResearchContext.LMS
{
    public class ProductReview
    {
        public int Id { get; set; }
        public Guid UserPublicKey { get; set; }
        public int ProductId { get; set; }
        public string ReviewText { get; set; }
        public bool IsAdminApproved { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedOn { get; set; }
    }

}
