namespace LMS.API.Models.ResponseModel
{
    public class ProductsLMSResponseModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? DescriptionTitle { get; set; }
        public string? CategoryName { get; set; }
        public string? GroupName { get; set; }
        public decimal? Price { get; set; }
        public string? LMSListImage { get; set; }

        public int? VideoCount { get; set; }

        // Subscription fields MUST be nullable
        public int? Months { get; set; }
        public decimal? ActualPrice { get; set; }
        public decimal? DiscountPrice { get; set; }
        public decimal? NetPayment { get; set; }

        public int? TotalCount { get; set; }
    }



}