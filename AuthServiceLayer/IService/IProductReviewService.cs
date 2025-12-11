using AuthServiceLayer.Models;
using LMS.API.Models.ResponseModel;

namespace LMS.API.IService
{
    public interface IProductReviewService
    {
        Task<ApiCommonResponseModel> AddReviewAsync(ReviewRequestModel model);
        Task<ApiCommonResponseModel> UpdateReviewAsync(int productId, ReviewRequestUpdateModel model);
        Task<ApiCommonResponseModel> DeleteReviewAsync(Guid userPublicKey, int productId);
        Task<ApiCommonResponseModel> GetReviewAsync(Guid userPublicKey, int productId);

        Task<ApiCommonResponseModel> GetReviewsByProductAsync(int productId);
           Task<ApiCommonResponseModel> GetRatingAsync(Guid createdBy, int productId);

    }

}
