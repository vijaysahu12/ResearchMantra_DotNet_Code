using AuthServiceLayer.Models;
using LMS.API.Models.ResponseModel;

namespace LMS.API.IService
{
    public interface ICartService
    {
        Task<ApiCommonResponseModel> AddToCartAsync(AddToCartRequestDto model) ;
        Task<ApiCommonResponseModel> GetUserCartAsync(Guid userPublicKey);
        Task<ApiCommonResponseModel> DeleteCartItemAsync(string userId, int productId);
    }
}
