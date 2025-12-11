using ProductService.Models.RequestModel;
using ProductService.Models.ResponseModel;

namespace ProductService.IServices
{
    public interface IResearchService
    {
        Task<ApiCommonResponseModel> GetBaskets(Guid loggedInUser);
        Task<ApiCommonResponseModel> GetCompanies(QueryValues param);
        Task<ApiCommonResponseModel> GetMessage(int id);
        Task<ApiCommonResponseModel> ManageComment(QueryValues request);
        Task<ApiCommonResponseModel> GetCompanyReport(QueryValues param);
    }
}
