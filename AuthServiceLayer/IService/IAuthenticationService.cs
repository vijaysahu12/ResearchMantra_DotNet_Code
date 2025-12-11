
using LMS.API.Controllers.AuthenticationFolder;
using AuthServiceLayer.Models;
using AuthServiceLayer.Models.RequestModel;

namespace LMS.API.IService
{
    public interface IAuthenticationService
    {
        Task<ApiCommonResponseModel> OtpLogin(string mobileNumber, string countryCode);

        Task<ApiCommonResponseModel> OtpLoginVerificationAndGetSubscription(
            MobileOtpVerificationRequestModel paramRequest);

        //Task<ApiCommonResponseModel> ManageUserDetails(ManageUserDetailsRequestModel queryValues);

        public Task<bool> CheckUserDetails(string userId);

        //Task<string?> GetUserFullNameAsync(string userId);
        Task<int> GetUserCartCount(string userId);
        Task<ApiCommonResponseModel> GetUserBasicDetailsAsync(string userId);
        Task<bool> IsProductInUserCartAsync(string userId, int productId);
        Task<CartProductStatus> GetProductStatusAsync(string userId, int productId);
    }
}
