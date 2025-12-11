using System;

namespace KRCRM.Database.KingResearchContext
{
    public class ProductsRatingM
    {
        public long Id { get; set; }
        public int ProductId { get; set; }
        public double Rating { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool? IsDelete { get; set; }
    }
}
