using ProductService.Models.RequestModel;
using ProductService.Models.ResponseModel;

namespace ProductService.IServices
{
    public interface IProductService
    {
        Task<ApiCommonResponseModel> GetProducts(string searchText, string userKey);

        Task<ApiCommonResponseModel> GetProductById(string id, string mobileUserKey);

        Task<ApiCommonResponseModel> LikeUnlikeProduct(LikeProductModel likedByRequest);

        Task<ApiCommonResponseModel> MyBucketContent(Guid userKey);

        Task<ApiCommonResponseModel> RateProduct(string userKey, int productId, float rating);

        Task<ApiCommonResponseModel> GetTopGainers(string type);

        Task<ApiCommonResponseModel> GetProductCategories();

        Task<ApiCommonResponseModel> GetProductContent(string userkey, int id);

        Task<ApiCommonResponseModel> GetProductContentV2(string userkey, int id);

        Task<ApiCommonResponseModel> GetLearningMaterial(Guid? MobileUserKey);

        Task<ApiCommonResponseModel> GetLearningContentList(GetLearningContentList request);

        Task<ApiCommonResponseModel> GetLearningContentById(int contentId, Guid mobileUserKey);

        Task<ApiCommonResponseModel> GetLearningContentExamples(int contentId);

        Task<ApiCommonResponseModel> LikeUnlikeLearningContent(LikeUnlikeLearningContentRequestModel request);

        Task<ApiCommonResponseModel> GetCoupons(Guid? userKey, int productId, int subscriptionDurationId);
        Task<ApiCommonResponseModel> ActiveProductCount();
        Task<ApiCommonResponseModel> ActiveProductStatus(Guid userKey);
        Task<ApiCommonResponseModel> GetPlayList(long mobileUserId, int productId);
        Task<ApiCommonResponseModel> AddQueryFormAsync(QueryFormRequestModel request);
        Task<ApiCommonResponseModel> GetActiveProductsByCategoryAsync(int categoryId);
    }
}
public class GetLearningContentList
{
    public int Id { get; set; }
    public Guid UserKey { get; set; }
}

public class QueryFormRequestModel
{
    public int Id { get; set; }
    public Guid MobileUserId { get; set; }
    public string ProductId { get; set; }
    public string QueryDescription { get; set; }
    public IFormFile? ScreenshotUrl { get; set; }
    public int QueryCategory { get; set; }
    public string? Remarks { get; set; }
    public int CreatedBy { get; set; }
    public int Status { get; set; }
}