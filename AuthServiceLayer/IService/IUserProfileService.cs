using AuthServiceLayer.Models;
using LMS.API.Models.RequestModel;

namespace LMS.API.IService
{
    public interface IUserProfileService
    {
        Task<ApiCommonResponseModel> GetUserProfileAsync(string userId);
        Task<ApiCommonResponseModel> UpdateUserProfileAsync(string userId, UpdateProfileRequest request);
        Task<ApiCommonResponseModel> UpdateProfilePictureAsync(string userId, IFormFile file);
        Task<ApiCommonResponseModel> DeleteProfilePictureAsync(string userId);
        Task<ApiCommonResponseModel> DeleteUserAccountAsync(string userId);
    }
}
