using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using LMS.API.IService;
using LMS.API.Service;
using AuthServiceLayer.Models.RequestModel;

namespace LMS.API.Controllers.AuthenticationFolder
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService  = authenticationService;
        }


        /// <summary>
        /// It check weather the number is in the database and then 
        /// </summary>
        /// <param name="mobileNumber"></param>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("OtpLogin")]
        public async Task<IActionResult> OtpLogin([Required] string mobileNumber, [Required] string countryCode)
        {
            return Ok(await _authenticationService.OtpLogin(mobileNumber, countryCode));
        }

        /// <summary>
        /// This method handles the OTP verification process for mobile users through whatsapp.
        /// It verifies the OTP sent to the user's mobile number in whatsapp and returns a response based on the result of the verification attempt.
        /// If the users fails to verify the otp 3 times then user has to wait for 30 mins and then retry.
        /// If the user logging in to a new device then it will send a notification to the old device to logout.
        /// Here access token is generated and refresh is updated only if it doesnt't exist.
        /// SP used: OtpLoginVerificationAndGetSubscription
        /// Table used : MobileUsers, Leads, MyBucketM
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [Route("OtpLoginVerificationAndGetSubscription")]
        public async Task<IActionResult> OtpLoginVerificationAndGetSubscription([FromBody] MobileOtpVerificationRequestModel paramRequest)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _authenticationService.OtpLoginVerificationAndGetSubscription(paramRequest));
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To update the user details like profile image, name, email, city, etc.
        /// From here the new user login will receive a welcome message in whatsapp along with mail.
        /// </summary>
        //[AllowAnonymous]
        //[HttpPost("ManageUserDetails")]
        //public async Task<IActionResult> ManageUserDetails([FromForm] ManageUserDetailsRequestModel queryValues)
        //{
        //    return Ok(await _authenticationService.ManageUserDetails(queryValues));
        //}

        [AllowAnonymous]
        [HttpPost("CheckUserDetails")]
        public async Task<IActionResult> CheckUserDetails(string userId)
        {
            return Ok(await _authenticationService.CheckUserDetails(userId));
        }

        [Authorize]
        [HttpGet("GetUserBasicDetailsAsync")]
        public async Task<IActionResult> GetUserBasicDetailsAsync([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("UserId is required.");

            var result = await _authenticationService.GetUserBasicDetailsAsync(userId);

            return Ok(result);
        }


        [Authorize]
        [HttpGet("GetUserCartCount")]
        public async Task<IActionResult> GetUserCartCount([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("UserId is required.");

            var cartCount = await _authenticationService.GetUserCartCount(userId);

            return Ok(new { UserId = userId, cartCount = cartCount });
        }

        [HttpGet("CheckProductInCart")]
        public async Task<IActionResult> CheckProductInCart(string userId, int productId)
        {
            if (userId == null || productId <= 0)
                return BadRequest(new { message = "Invalid userId or productId." });

            bool exists = await _authenticationService.IsProductInUserCartAsync(userId, productId);

            if (exists)
                return Ok(new { exists = true, message = "Product already in cart." });
            else
                return Ok(new { exists = false, message = "Product not in cart." });
        }

        [HttpGet("GetProductStatus")]
        public async Task<IActionResult> GetProductStatus(string userId, int productId)
        {
            if (string.IsNullOrEmpty(userId) || productId <= 0)
                return BadRequest(new { message = "Invalid userId or productId." });

            var status = await _authenticationService.GetProductStatusAsync(userId, productId);

            return Ok(new
            {
                status = status.ToString(),
                PurchasedStatusCode = (int)status
            });
        }

    }
    public enum CartProductStatus
    {
        Open = 1,        // Not in cart, not purchased
        InCart = 2,      // In cart but not purchased
        Purchased = 3    // Already purchased
    }

}
