
using AuthServiceLayer.Models;
using LMS.API.Models.ResponseModel;

namespace LMS.API.IService
{
    public interface IProductService
    {
        Task<ApiCommonResponseModel> GetAllProducts(int? categoryId, string? SearchText, int pageNumber = 1, int pageSize = 10);
        Task<ApiCommonResponseModel> GetProductsDetail(int productId);
        Task<ApiCommonResponseModel> GetProductContents(int productId, Guid? userId);
        Task<ApiCommonResponseModel> GetTop4ProductList(int userId);

        Task<ApiCommonResponseModel> GetMyBucketList(Guid userKey);
        Task<ApiCommonResponseModel> LikeUnlikeProduct(LikeProductRequestModel likedByRequest);

        Task<ApiCommonResponseModel> RateProduct(string userKey, int productId, float rating);

        
    }
}
