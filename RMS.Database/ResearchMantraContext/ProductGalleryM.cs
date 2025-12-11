using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRCRM.Database.KingResearchContext
{
    public class ProductGalleryM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
        public string AspectRatio { get; set; } = "auto";
        public bool IsActive { get; set; } = true;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? ModifiedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
