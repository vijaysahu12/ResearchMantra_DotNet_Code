namespace LMS.API.Models.ResponseModel
{
    public class ProductsDetailLMSResponseModel
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? DescriptionTitle { get; set; }
        public string? CategoryName { get; set; }
        public string? GroupName { get; set; }
        public decimal Price { get; set; }
        public string? UserRating { get; set; }
        public string? ListImage { get; set; }
        public string? LandScapeImg { get; set; }
        //public string? LmsImage { get; set; }
        public int VideoCount { get; set; }   
        public int? Months { get; set; }   
        public decimal? DiscountPrice { get; set; }   
        public decimal? ActualPrice { get; set; }  
        public decimal? NetPayment { get; set; }   
        public decimal? DiscountPercentage { get; set; }  
        public int? CommunityId { get; set; }
        public string? CommunityName { get; set; }
        public int? ScannerBonusId { get; set; }
        public string? ScannerBonusName { get; set; }
    }

    public class TopProductResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public double AverageRating { get; set; }

        public string? LMSListImage { get; set; }

        public string? CategoryName { get; set; }
    }

}
