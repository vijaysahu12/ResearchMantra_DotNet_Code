using KRCRM.Database.KingResearchContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Helper;
using ProductService.IServices;
using ProductService.Models.RequestModel;

namespace ProductService.Controllers.ResearchReportFolder
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResearchController : ControllerBase
    {
        readonly IResearchService _researchService;
        public ResearchController(IResearchService researchService)
        {
            _researchService = researchService;
        }

        [HttpGet("GetBaskets")]
        public async Task<IActionResult> GetBaskets()
        {
            _ = Guid.TryParse(UserClaimsHelper.GetClaimValue(User, "userPublicKey"), out Guid loggedInUser);
            return Ok(await _researchService.GetBaskets(loggedInUser));
        }

        [HttpPost("GetCompanies")]
        public async Task<IActionResult> GetCompanies(QueryValues param)
        {
            return Ok(await _researchService.GetCompanies(param));
        }

        [HttpPost("GetCompanyReport")]
        public async Task<IActionResult> GetCompanyReport(QueryValues param)
        {
            return Ok(await _researchService.GetCompanyReport(param));
        }

        [HttpGet("GetComments")]
        public async Task<IActionResult> GetMessage(int companyId)
        {
            return Ok(await _researchService.GetMessage(companyId));
        }

        [HttpPost("ManageComment")]
        public async Task<IActionResult> ManageComment(QueryValues request)
        {
            return Ok(await _researchService.ManageComment(request));
        }


    }
}
