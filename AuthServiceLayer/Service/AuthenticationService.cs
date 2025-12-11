using KRCRM.Database.Constants;
using System.Data;
using System.Net;
using KRCRM.Database.KingResearchContext;

using LMS.API.IService;
using Microsoft.Data.SqlClient;
using KRCRM.Database.MongoDbContext;
using KRCRM.Database.Extension;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LMS.API.Controllers.AuthenticationFolder;
using AuthServiceLayer.Models;
using AuthServiceLayer.IService;
using AuthServiceLayer.Models.ResponseModel;
using AuthServiceLayer.Models.RequestModel;
using Newtonsoft.Json;


namespace LMS.API.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly KingResearchContext _context;
        private readonly ApiCommonResponseModel responseModel = new();
        //private readonly IMobileNotificationService _mobileNotificationService;
        private readonly IConfiguration _config;
        private readonly int _tokenExpiryInDays = 20;
        public AuthenticationService(KingResearchContext context, IConfiguration config
           )
        {
            _context = context;
            _config = config;
            //_mobileNotificationService = mobileNotificationService;
            
        }


        public async Task<ApiCommonResponseModel> OtpLogin(string mobileNumber, string countryCode)
        {
            string? createdBy = _config.GetSection("AppSettings:DefaultAdmin").Value;

            SqlParameter otp = new()
            {
                ParameterName = "Otp",
                SqlDbType = SqlDbType.VarChar,
                Size = 6,
                Direction = ParameterDirection.Output,
            };

            List<SqlParameter> sqlParameters =
            [
                new SqlParameter
                {
                    ParameterName = "MobileNumber",
                    Value = mobileNumber == "" ? DBNull.Value : mobileNumber,
                    SqlDbType = SqlDbType.VarChar
                },
                new SqlParameter
                {
                    ParameterName = "CreatedBy",
                    Value = Guid.Parse(createdBy),
                    SqlDbType = SqlDbType.UniqueIdentifier
                },
                new SqlParameter
                {
                    ParameterName = "ModifiedBy",
                    Value = Guid.Parse(createdBy),
                    SqlDbType = SqlDbType.UniqueIdentifier
                },
                new SqlParameter { ParameterName = "DeviceType", Value = "Android", SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "CountryCode", Value = countryCode, SqlDbType = SqlDbType.VarChar },
                otp
            ];

            OtpLogin result =
                await _context.SqlQueryFirstOrDefaultAsync2<OtpLogin>(ProcedureCommonSqlParametersText.OtpLogin, sqlParameters.ToArray());
            result.ProfileImage = result.ProfileImage != null ? _config["Azure:ImageUrlSuffix"] + result.ProfileImage : null;

            if (result.Result.ToUpper() == "USERDELETED")
            {
                responseModel.StatusCode = HttpStatusCode.NotFound;
                responseModel.Message = result.Message;
            }

            if (result.Result.ToUpper() == "OTPLIMITREACHED")
            {
                responseModel.StatusCode = HttpStatusCode.Forbidden;
                responseModel.Message = result.Message;
            }

            if (result.Result.ToUpper() == "OTPSENT")
            {
                try
                {
                   
                        SendOTP(mobileNumber, otp.Value.ToString()!, countryCode);
                    
                    //await _mongoService.DeleteSelfAccountRequestedData(mobileNumber); //Delete from mongodb if he already requested for self account's delete
                }
                catch (Exception ex)
                {
                    //await _exception.AddAsync(new ExceptionLog
                    //{
                    //    CreatedOn = DateTime.Now,
                    //    InnerException = ex.InnerException?.Message,
                    //    Message = ex.Message,
                    //    RequestBody = $"MobileNumber: {mobileNumber}, CountryCode: {countryCode}",
                    //    Source = "OTPSENT",
                    //    StackTrace = ex.StackTrace
                    //});
                    //await _mobileNotificationService.SendErrorNotificationAsync("OTPSENT", ex.Message);
                    throw ex;
                }

                responseModel.StatusCode = HttpStatusCode.OK;
                responseModel.Message = result.Message;
            }

            if (result.Result.ToUpper() == "REGISTERED")
            {
                SendOTP(mobileNumber, otp.Value.ToString()!, countryCode);
                responseModel.StatusCode = HttpStatusCode.OK;
                responseModel.Message = result.Message;
            }

            responseModel.Message = result.Message;
            responseModel.Data = result;
            return responseModel;
        }

        public async Task<ApiCommonResponseModel> OtpLoginVerificationAndGetSubscription(MobileOtpVerificationRequestModel paramRequest)
        {
            SqlParameter oldDeviceFcmToken = new()
            {
                ParameterName = "oldDeviceFcmToken",
                Direction = ParameterDirection.Output,
                SqlDbType = SqlDbType.VarChar,
                Size = 1000
            };

            #region otp verificatoin through sp [OtpLoginVerification]

            List<SqlParameter> sqlParameters =
            [
                new SqlParameter
                {
                    ParameterName = "MobileUserKey",
                    Value = paramRequest.MobileUserKey == "" ? DBNull.Value : Guid.Parse(paramRequest.MobileUserKey),
                    SqlDbType = SqlDbType.UniqueIdentifier
                },
                new SqlParameter
                {
                    ParameterName = "FirebaseFcmToken",
                    Value = string.IsNullOrEmpty(paramRequest.FirebaseFcmToken)
                        ? DBNull.Value
                        : paramRequest.FirebaseFcmToken,
                    SqlDbType = SqlDbType.VarChar
                },
                new SqlParameter
                {
                    ParameterName = "DeviceType",
                    Value = paramRequest.DeviceType == "" ? DBNull.Value : paramRequest.DeviceType,
                    SqlDbType = SqlDbType.VarChar
                },
                new SqlParameter
                { ParameterName = "Otp", Value = paramRequest.Otp, SqlDbType = SqlDbType.VarChar, Size = 6 },
                oldDeviceFcmToken
            ];

            OtpLoginVerificationResponse result =
                await _context.SqlQueryFirstOrDefaultAsync2<OtpLoginVerificationResponse>(ProcedureCommonSqlParametersText.OtpLoginVerificationAndGetSubscription, sqlParameters.ToArray());

            #endregion otp verificatoin through sp [OtpLoginVerification]

            // if new device is used to login then fcm token is going to change and if that changes we will send a notification to the old device which will logout from the device
            //if (oldDeviceFcmToken.Value != null && !string.IsNullOrEmpty(oldDeviceFcmToken.Value.ToString()))
            //{
            //    await _mobileNotificationService.SendNotificationToTokenToLogoutFromOldDevice(oldDeviceFcmToken.Value.ToString());
            //}

            if (result.Result?.ToUpper() == "VERIFIEDUSER")
            {
                responseModel.StatusCode = HttpStatusCode.OK;
                responseModel.Message = result.Message;

                //Revoke all the old token
                //await _context.UserRefreshTokenM.Where(token => token.MobileUserKey == result.MobileUserKey && token.IsRevoked == false)
                //    .ExecuteUpdateAsync(update => update.SetProperty(token => token.IsRevoked, true));

                string token = GenerateJWTToken(result.MobileUserKey.ToString(), result.LeadKey, result.MobileUserId, paramRequest.DeviceType, paramRequest.Version, _tokenExpiryInDays);// JWT Token Generate
                var newRefreshToken = GenerateRefreshToken();

                if (string.IsNullOrEmpty(newRefreshToken))
                {
                    //await _log.AddAsync(new Log { CreatedOn = DateTime.Now, Message = "RefreshToken is null ", Source = "RefreshToken", Category = "JWT" });
                }
                var refreshTokenEntry = await _context.UserRefreshTokenM.Where(x => x.MobileUserKey == result.MobileUserKey).FirstOrDefaultAsync();

                if (refreshTokenEntry != null)
                {
                    refreshTokenEntry.RefreshToken = newRefreshToken;
                    refreshTokenEntry.IssuedAt = DateTime.Now;
                    refreshTokenEntry.ExpiresAt = DateTime.Now.AddDays(20);
                    refreshTokenEntry.IsRevoked = false;
                    _context.UserRefreshTokenM.Update(refreshTokenEntry);
                }
                else
                {
                    _ = _context.UserRefreshTokenM.Add(new UserRefreshTokenM
                    {
                        MobileUserKey = result.MobileUserKey,
                        RefreshToken = newRefreshToken,
                        IssuedAt = DateTime.Now,
                        ExpiresAt = DateTime.Now.AddDays(20),
                        DeviceType = paramRequest.DeviceType,
                        IsRevoked = false,
                    });

                }

                _ = await _context.SaveChangesAsync();
                //if (string.IsNullOrEmpty(newRefreshToken))
                //{
                //    await _log.AddAsync(new Log { CreatedOn = DateTime.Now, Message = $"NewRefreshToken Not Generated for {result.MobileNumber}", Source = "JWT", Category = "JWT" });
                //}
                responseModel.Data = new
                {
                    publicKey = result.MobileUserKey,
                    mobileUserId = result.MobileUserId,
                    name = result.FullName,
                    Token = token,
                    RefreshToken = newRefreshToken,
                    AccessToken = token,
                    profileImage = result.ProfileImage,
                    SubscriptionTopics = result.EventSubscription,
                    result.IsExistingUser,
                    result.Gender,
                    result.IsFreeTrialActivated,
                    ExpiresAt = DateTime.Now.AddDays(20)
                };
            }
            else if (result.Result?.ToUpper() == "OTPFAILED")
            {
                responseModel.StatusCode = HttpStatusCode.Unauthorized;
                responseModel.Message = result.Message;
                responseModel.Data = null;
            }
            else if (result.Result?.ToUpper() == "USERDELETED")
            {
                responseModel.StatusCode = HttpStatusCode.NotFound;
                responseModel.Message = result.Message;
                responseModel.Data = null;
            }
            else if (result.Result.ToUpper() == "RETRYATTEMPTLIMITREACHED")
            {
                responseModel.StatusCode = HttpStatusCode.Forbidden;
                responseModel.Message = result.Message;
                responseModel.Data = null;
            }
            else if (result.Result is null)
            {
                responseModel.StatusCode = HttpStatusCode.NotFound;
                responseModel.Message = "User Not Found.";
                responseModel.Data = null;
            }

            return responseModel;
        }

        //public async Task<ApiCommonResponseModel> ManageUserDetails(ManageUserDetailsRequestModel obj)
        //{
        //    MobileUser? existingUserWithEmail;
        //    MobileUser? mobileUserDetails =
        //        await _context.MobileUsers.FirstOrDefaultAsync(item => item.PublicKey == Guid.Parse(obj.PublicKey) && item.IsDelete == false);
        //    if (mobileUserDetails != null)
        //    {
        //        existingUserWithEmail = await _context.MobileUsers.FirstOrDefaultAsync(item =>
        //            item.EmailId == obj.EmailId && item.PublicKey != mobileUserDetails.PublicKey && item.IsDelete == false);
        //    }
        //    else
        //    {
        //        responseModel.Message = "User Not Found.";
        //        responseModel.StatusCode = HttpStatusCode.NotFound;
        //        return responseModel;
        //    }

        //    if (existingUserWithEmail != null)
        //    {
        //        responseModel.Message = "EmailId already exists.";
        //        responseModel.StatusCode = HttpStatusCode.BadRequest;
        //        return responseModel;
        //    }

        //    int maxAllowedSizeInBytes = 31457280;
        //    bool profileImageExists = false;

        //    if (obj.ProfileImage != null)
        //    {
        //        profileImageExists = true;
        //        if (obj.ProfileImage.Length > maxAllowedSizeInBytes)
        //        {
        //            responseModel.StatusCode = HttpStatusCode.BadRequest;
        //            responseModel.Message = $"File Size cannot be more than {maxAllowedSizeInBytes / 1048576} MB";
        //            return responseModel;
        //        }
        //    }

        //    if (mobileUserDetails is { IsOtpVerified: true, IsActive: true })
        //    {
        //        if (string.IsNullOrEmpty(mobileUserDetails.FullName?.Trim()))
        //        {
        //            //try
        //            //{
        //            //await SendWhatsappWelcomeAsync(
        //            //    mobileUserDetails.Mobile,
        //            //    obj.FullName,
        //            //    mobileUserDetails.CountryCode);
        //            //await _emailService.SendWelcomeEmailAsync(obj.EmailId, obj.FullName);
        //            //}
        //            //catch (Exception ex)
        //            //{
        //            //    await _exception.AddAsync(new ExceptionLog
        //            //    {
        //            //        Source = "ManageUserDetails",
        //            //        RequestBody = JsonSerializer.Serialize(obj),
        //            //        Message = ex.Message,
        //            //        InnerException = ex.InnerException?.Message,
        //            //        StackTrace = ex.StackTrace,
        //            //        CreatedOn = DateTime.Now
        //            //    });
        //            //}
        //        }
        //        if (mobileUserDetails.ProfileImage is not null && profileImageExists)
        //            _ = await _azureBlobStorageService.DeleteImage(mobileUserDetails.ProfileImage);

        //        mobileUserDetails.FullName = obj.FullName;
        //        mobileUserDetails.City = obj.City;
        //        mobileUserDetails.Gender = obj.Gender;
        //        mobileUserDetails.Dob = DateTime.Parse(obj.Dob);
        //        mobileUserDetails.EmailId = obj.EmailId;
        //        mobileUserDetails.ProfileImage = profileImageExists
        //            ? await _azureBlobStorageService.UploadImage(obj.ProfileImage!)
        //            : mobileUserDetails.ProfileImage;
        //        Lead? leadDetails =
        //                              await _context.Leads.FirstOrDefaultAsync(item => item.MobileNumber == obj.Mobile);

        //        if (leadDetails != null)
        //        {
        //            leadDetails.FullName = mobileUserDetails.FullName;
        //            leadDetails.EmailId = mobileUserDetails.EmailId;
        //            leadDetails.Gender = mobileUserDetails.Gender == "male" ? "M" : "F";

        //            if (mobileUserDetails.LeadKey == Guid.Empty)
        //            {
        //                mobileUserDetails.LeadKey = leadDetails?.PublicKey ?? Guid.Empty;
        //            }
        //        }
        //        await _context.SaveChangesAsync();

        //        // update profile image in mongo
        //        _ = await _mongoService.UpdateProfileImage(mobileUserDetails.PublicKey, mobileUserDetails.ProfileImage);

        //        responseModel.Message = "Update Successfull.";
        //        responseModel.StatusCode = HttpStatusCode.OK;
        //    }
        //    else if (mobileUserDetails is not null &&
        //             (mobileUserDetails.IsActive == false && mobileUserDetails.IsDelete == true))
        //    {
        //        responseModel.Message = "User deleted.";
        //        responseModel.StatusCode = HttpStatusCode.NotFound;
        //    }
        //    else if (mobileUserDetails is not null && mobileUserDetails.IsOtpVerified == false &&
        //             mobileUserDetails.IsActive == true)
        //    {
        //        responseModel.Message = "Mobile Number is not Verified.";
        //        responseModel.StatusCode = HttpStatusCode.Forbidden;
        //    }

        //    return responseModel;
        //}

        public async Task<ApiCommonResponseModel> GetUserBasicDetailsAsync(string userId)
        {
            var response = new ApiCommonResponseModel();

            try
            {
                var user = await _context.MobileUsers
                    .Where(u => u.PublicKey.ToString() == userId)
                    .Select(u => new
                    {
                        FullName = u.FullName,
                        Email = u.EmailId,
                        Mobile = u.Mobile
                    })
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "User not found";
                    response.Data = null;
                    return response;
                }

                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Success";
                response.Data = user;
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                response.Data = null;
            }

            return response;
        }

        public async Task<int> GetUserCartCount(string userId)
        {
            var count = await _context.AddToCart
                .Where(c =>
                    c.UserPublicKey.ToString() == userId
                    && c.IsActive
                    && !c.IsDelete
                    && !c.IsPurchased
                    && !_context.MyBucketM.Any(m =>
                           m.MobileUserKey.ToString() == userId
                           && m.ProductId == c.ProductMId
                       )
                )
                .CountAsync();

            return count;
        }


        public async Task<CartProductStatus> GetProductStatusAsync(string userId, int productId)
        {
            var item = await _context.AddToCart
                .FirstOrDefaultAsync(c => c.UserPublicKey.ToString() == userId
                                       && c.ProductMId == productId
                                       && !c.IsDelete
                                       && c.IsActive);
            var isPurchesed = await _context.MyBucketM
               .FirstOrDefaultAsync(c => c.MobileUserKey.ToString()== userId
                                      && c.ProductId == productId
                                      && !c.IsExpired
                                      && c.IsActive);

            if (isPurchesed != null)
                return CartProductStatus.Purchased;
            // Case 1: Not in cart
            if (item == null)
                return CartProductStatus.Open;

            // Case 2: Purchased
            

            // Case 3: In cart but not purchased
            return CartProductStatus.InCart;
        }

        public async void SendOTP(string mobileNumber, string otp, string countryCode)
        {
            try
            {
                // ✅ Normalize the country code
                if (!countryCode.StartsWith("+"))
                    countryCode = "+" + countryCode;

                var payload = new
                {
                    from = _config["AOC:SenderNumber"],             // e.g. "+917075506186"
                    campaignName = _config["AOC:CampaignName"],     // e.g. "api-test"
                    to = $"{countryCode}{mobileNumber.Trim()}",     // e.g. "+919876543210"
                    templateName = _config["AOC:TemplateName"],     // e.g. "otp"
                    otp = otp,                                      // your dynamic OTP
                    type = "template",
                    language = new { code = "en" }

                };

                var json = JsonConvert.SerializeObject(payload);
                using var httpClient = new HttpClient();

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.aoc-portal.com/v1/whatsapp");
                httpRequest.Headers.Add("apikey", _config["AOC:ApiKey"]);
                httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.SendAsync(httpRequest);
                var responseBody = await response.Content.ReadAsStringAsync();

                //if (!response.IsSuccessStatusCode)
                //{
                //    // ❌ Log failure
                //    await _log.AddAsync(new Log
                //    {
                //        CreatedOn = DateTime.Now,
                //        Message = $"Failed to send OTP to {mobileNumber}. Status: {response.StatusCode}, Response: {responseBody}",
                //        Source = "AOC_SendOTP_Failed",
                //        Category = "Whatsapp"
                //    });
                //}
                //else
                //{
                //    // ✅ Log success (optional)
                //    // await _log.AddAsync(new Log
                //    // {
                //    //     CreatedOn = DateTime.Now,
                //    //     Message = $"OTP sent successfully to {mobileNumber}. Response: {responseBody}",
                //    //     Source = "AOC_SendOTP_Success",
                //    //     Category = "Whatsapp"
                //    // });
                //}
            }
            catch (Exception ex)
            {
                //await _exception.AddAsync(new ExceptionLog
                //{
                //    Source = "AOC_OTP",
                //    RequestBody = $"MobileNumber: {mobileNumber}, OTP: {otp}, CountryCode: {countryCode}",
                //    Message = ex.Message,
                //    InnerException = ex.InnerException?.Message,
                //    StackTrace = ex.StackTrace,
                //    CreatedOn = DateTime.Now
                //});

                //await _mobileNotificationService.SendErrorNotificationAsync("AOC_OTP", ex.Message);
                throw ex;
            }

        }
        public static string GenerateRefreshToken(int length = 64) // Increased default length
        {
            const string chars = "ABCDEFGHIJKLMVWX012345YZabcdeNOPQRSTUfghijklmnopqrstuvwxyz6789";
            var tokenBuilder = new StringBuilder(length);
            using (var rng = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[length];
                rng.GetBytes(randomBytes);
                for (int i = 0; i < length; i++)
                {
                    tokenBuilder.Append(chars[randomBytes[i] % chars.Length]);
                }
            }
            var base64Token = Convert.ToBase64String(Encoding.UTF8.GetBytes(tokenBuilder.ToString()));
            //Optional sanitization. Remove if not needed.
            var sanitizedToken = new StringBuilder(base64Token.Length);
            foreach (char c in base64Token)
            {
                if (char.IsLetterOrDigit(c))
                {
                    sanitizedToken.Append(c);
                }
            }
            return sanitizedToken.ToString();
        }

        private string GenerateJWTToken(string publicKey, string? leadKey, long mobileUserId, string deviceType, string version, int tokenExpiryInDays)
        {
            Claim[] claims =
 {
             new Claim(ClaimTypes.Role, publicKey),
             new Claim("userPublicKey", publicKey),
             new Claim("userLeadKey", leadKey ?? ""),
             new Claim("deviceType", deviceType ?? ""),
             new Claim("version", version ?? ""),
             new Claim(ClaimTypes.PrimarySid, mobileUserId.ToString()),
         };

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value!));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512Signature);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = creds,
                Expires = DateTime.Now.AddDays(_tokenExpiryInDays)
            };
            JwtSecurityTokenHandler tokenHandler = new();
            JwtSecurityToken token = (JwtSecurityToken)tokenHandler.CreateToken(tokenDescriptor);
            token.Header["kid"] = "my-signing-key-1234";
            return tokenHandler.WriteToken(token);


        }


        public async Task<bool> CheckUserDetails(string userId)
        {
                var user = await _context.MobileUsers
                    .Where(u => u.PublicKey.ToString() == userId)
                    .Select(u => new { u.FullName, u.EmailId })
                    .FirstOrDefaultAsync();

                if (user == null)
                    return false; // user does not exist

                return !string.IsNullOrWhiteSpace(user.FullName)
                    && !string.IsNullOrWhiteSpace(user.EmailId);
        }


        public async Task<bool> IsProductInUserCartAsync(string userId, int productId)
        {
            var exists = await _context.AddToCart
                .AnyAsync(c => c.UserPublicKey.ToString() == userId && c.ProductMId == productId && !c.IsDelete && c.IsActive);

            return exists;
        }
    }
}
