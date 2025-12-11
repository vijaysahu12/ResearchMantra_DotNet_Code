
using AuthServiceLayer.Models;
using static LMS.API.Models.ResponseModel.LMSSubscriptionModel;

namespace LMS.API.IService
{
    public interface ISubscriptionService
    {
        Task<ApiCommonResponseModel> GetSubscriptionById(SubscriptionRequestModel request);

        //Task<ApiCommonResponseModel> ValidateCoupon(ValidateCouponRequestModel request);
    }
}
