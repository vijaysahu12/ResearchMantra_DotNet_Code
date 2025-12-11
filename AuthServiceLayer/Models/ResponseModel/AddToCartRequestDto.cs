namespace LMS.API.Models.ResponseModel
{

    public class AddToCartResponseDto
    {
        public int CartId { get; set; }                  

        // From Products table
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        public string? DescriptionTitle { get; set; }
        //public decimal Price { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public string? ImageUrl { get; set; }
        public string? LMSListImage { get; set; }
        public string? LandscapeImage { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public bool IsImportant { get; set; }
        //public int? Months { get; set; }
        public decimal? ActualPrice { get; set; }
        //public decimal? DiscountPrice { get; set; }
        public decimal? NetPayment { get; set; }

    }

    public class AddToCartRequestDto
    {
        public Guid UserPublicKey { get; set; }
        public List<int> ProductIds { get; set; } = new();
        public string? CreatedBy { get; set; }
    }

}
