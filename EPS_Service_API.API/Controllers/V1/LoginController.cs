using EPS_Service_API.API.Data;
using EPS_Service_API.API.RefreshTokenConfig;
using EPS_Service_API.API.Repositories;
using EPS_Service_API.Model;
using EPS_Service_API.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EPS_Service_API.API.V1.Controllers
{
    [ApiVersion("1.0")]

    [ApiController]
    public class LoginController : ControllerBase
    {
        private SecurityHelper _securityHelper;
        private ILoginRepository LoginRepository;
        private readonly ILogger<LoginController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _context;
        private readonly JwtBearerTokenSettings jwtBearerTokenSettings;
        private DeviceValidator _DeviceValidator;

        public LoginController(
            ILoginRepository LoginRepository,
            SecurityHelper securityHelper,
            ILogger<LoginController> logger,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IOptions<JwtBearerTokenSettings> jwtTokenOptions,
             DeviceValidator DeviceValidator
            )
        {
            this.LoginRepository = LoginRepository;
            _securityHelper = securityHelper;
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            this.jwtBearerTokenSettings = jwtTokenOptions.Value;
            _DeviceValidator = DeviceValidator;
        }



        [HttpPost]
        [Route("~/api/v{version:apiVersion}/Login/UserLogin")]
        public async Task<IActionResult> UserLogin([FromBody] UserLoginModel userLogin)
        {
            string hash = Request.Headers["x-hash"].ToString(); // Check if the hash is valid

            string _MobileNo = _DeviceValidator.FixPhoneNo(userLogin.UserName);
            string _Password = userLogin.Password;

            SetMobileNumberInCookie(_MobileNo);
            SetDeviceIDInCookie(userLogin.DeviceID);

            var result_PassWordLogin = await _signInManager.PasswordSignInAsync(_MobileNo, _Password, false, lockoutOnFailure: true);

            IActionResult response = Unauthorized();
            objUserLoginProfileModel _objResponseModel = new objUserLoginProfileModel();

            if (result_PassWordLogin.Succeeded)
            {
                #region Token Creation Part

                try
                {



                    var result = await LoginRepository.Login_byMobileNo(_MobileNo, _Password);

                    if (result.CustomerID < 1) //Handle Incomplete Registration Attempt
                    {
                        _objResponseModel.UserProfile = null;
                        _objResponseModel.Token = null;
                        _objResponseModel.TokenExpiration = null;
                        //   _objResponseModel.RefreshToken = null;
                        _objResponseModel.StatusCode = 1;
                        _objResponseModel.ErrorDescription = "Registration not complete! Please register.";
                        _objResponseModel.APIVersion = "0.1";
                        return Created("Result", _objResponseModel);
                    }

                    //Hold Dynamic User
                    //   var user = await _userManager.FindByNameAsync(_MobileNo);
                    // var identityUser = new ApplicationUser() { UserName = userDetails.UserName, Email = userDetails.Email };
                    var identityUser = new ApplicationUser() { UserName = _MobileNo };

                    #region Token Generator

                    UserInfoModel userInfo = new UserInfoModel();

                    string tokenString = null;

                    string RefreshtokenString = null; //For Refresh Token
                    
                    if (result != null)
                    {
                        userInfo.UserName = result.MobileNumber;
                        userInfo.Password = userLogin.Password;

                        if (result.CustomerName == null)
                        {
                            userInfo.Name = "No Information";
                        }
                        else if(result.CustomerName.Length > 2)
                        {
                            userInfo.Name = result.CustomerName;
                        }
                        else { userInfo.Name = "No Information"; }

                        if (result.EmailAddress==null)
                        {
                            userInfo.Email = "No Information";
                        }
                        else if (result.EmailAddress.Length > 2) { userInfo.Email = result.EmailAddress; }
                        else { userInfo.Email = "No Information"; }


                        userInfo.Role = "MobileUser";
                        userInfo.DeviceID = userLogin.DeviceID;
                    }


                    if (userInfo != null && userInfo.UserName != null)
                    {
                        tokenString = _securityHelper.GenerateJSONWebToken(userInfo);
                    }

                    #endregion

                    if (result == null)
                    {
                        _objResponseModel.UserProfile = null;
                        _objResponseModel.Token = null;
                        _objResponseModel.TokenExpiration = null;
                        //   _objResponseModel.RefreshToken = null;
                        _objResponseModel.StatusCode = 1;
                        _objResponseModel.ErrorDescription = "Not Found!";
                        _objResponseModel.APIVersion = "0.1";
                        return Created("Result", _objResponseModel);

                    }
                    else
                    {
                        
                   //     var Ref_tok= await Get_RefreshTokenAsync(_MobileNo, _Password);
                   //     RefreshtokenString = Ref_tok.Token;
                   //     SetRefreshTokenInCookie(RefreshtokenString);

                        //For Refresh Token

                        _objResponseModel.UserProfile = result;
                        _objResponseModel.Token = tokenString;
                        _objResponseModel.TokenExpiration = DateTime.Now.AddMinutes(+10).ToString("yyyy-MM-dd HH:mm");
                        //      _objResponseModel.RefreshToken = RefreshtokenString;
                        //      _objResponseModel.RefreshTokenExpiration = Ref_tok.Expires;
                        _objResponseModel.StatusCode = 200;
                        _objResponseModel.ErrorDescription = null;
                        _objResponseModel.APIVersion = "0.1";
                        //    return Ok( _objResponseModel);

                        // API Related Information  start

                        // Method mongodb

                        // API Related Information  start

                        return Created("Result", _objResponseModel);
                    }
                    
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "We caught this exception in UserLogin API.");
                    string errorDescription_ = ex.Message.ToString();
                    _objResponseModel.UserProfile = null;
                    _objResponseModel.StatusCode = 1;
                    // _objResponseModel.ErrorDescription = errorDescription_;
                    _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                    _objResponseModel.APIVersion = "0.1";
                    //   return BadRequest( _objResponseModel);
                    return Created("Result", _objResponseModel);
                }

                #endregion

            }

            if (result_PassWordLogin.RequiresTwoFactor)
            {
                
                _objResponseModel.UserProfile = null;
                _objResponseModel.Token = null;
                _objResponseModel.StatusCode = 1;
              //  _objResponseModel.ErrorDescription = "AccountWasNotVerified_TryRecreate";
                _objResponseModel.ErrorDescription = "Account is not verified! Please try to recreate.";
                _objResponseModel.APIVersion = "0.1";
                //   return BadRequest( _objResponseModel);
                return Created("Result", _objResponseModel);

            }

            if (result_PassWordLogin.IsLockedOut)
            {
                _objResponseModel.UserProfile = null;
                _objResponseModel.Token = null;
                _objResponseModel.StatusCode = 1;
               // _objResponseModel.ErrorDescription = "AccountLockedFor15Minutes";
                _objResponseModel.ErrorDescription = "Account Locked For 15 Minutes! Please try later.";
                _objResponseModel.APIVersion = "0.1";
                //  return Unauthorized( _objResponseModel);
                return Created("Result", _objResponseModel);

            }

            else
            {
                _objResponseModel.UserProfile = null;
                _objResponseModel.Token = null;
                _objResponseModel.StatusCode = 1;
                // _objResponseModel.ErrorDescription = "WrongMobileNumber_PasswordCombination";
                _objResponseModel.ErrorDescription = "No account matched for this number and PIN! Please register new number to use application.";
                _objResponseModel.APIVersion = "0.1";
                //  return BadRequest( _objResponseModel);
                return Created("Result", _objResponseModel);

            }

        }


        [HttpPost]
        [Route("~/api/v{version:apiVersion}/Logout")]
        public async Task<IActionResult> Logout()
        {
            string hash = Request.Headers["x-hash"].ToString(); // Check if the hash is valid
            string _mobileNumber= Request.Cookies["mobileNumber"];
            if (_mobileNumber != null)
            {
                await RevokeRefreshToken(_mobileNumber);
            }

            objUserLogoutModel _objResponseModel = new objUserLogoutModel();
            _objResponseModel.StatusCode = 200;
            _objResponseModel.ErrorDescription = "";
            _objResponseModel.APIVersion = "0.1";
            _objResponseModel.Message = "Logout Successful";

            //  return Ok( _objResponseModel);
              return Created("Result", _objResponseModel);
        }


        [HttpPost]
        [Route("~/api/v{version:apiVersion}/RenewAccessToken")]
        public async Task<IActionResult> RenewAccessToken()
        {
            string hash = Request.Headers["x-hash"].ToString(); // Check if the hash is valid
            string _refreshToken = Request.Cookies["refreshToken"];
            string _deviceId = Request.Cookies["deviceId"];
            objRenewAccessToken _objResponseModel = new objRenewAccessToken();
            var Ex_token = await LoginRepository.EPS_Service_Check_Refresh_Token_byToken(_refreshToken);

            if (Ex_token != null)
            {
                if (Ex_token.IsActive != false)
                {

                    var user1 = new ApplicationUser() { Id = Ex_token.ApplicationUserId };
                    var user = await _userManager.FindByIdAsync(user1.Id);

                    var result = await LoginRepository.Login_byMobileNo(user.PhoneNumber, "Dummy");

                    #region Token Generator

                    UserInfoModel userInfo = new UserInfoModel();

                    string tokenString = null;

                    if (result != null)
                    {
                        userInfo.UserName = result.MobileNumber;
                        userInfo.Password = "Dummy";

                        if (result.CustomerName == null)
                        {
                            userInfo.Name = "No Name Yet";
                        }
                        else if (result.CustomerName.Length > 2)
                        {
                            userInfo.Name = result.CustomerName;
                        }
                        else { userInfo.Name = "No Name Yet"; }

                        if (result.EmailAddress == null)
                        {
                            userInfo.Email = "No Email Yet";
                        }
                        else if (result.EmailAddress.Length > 2) { userInfo.Email = result.EmailAddress; }
                        else { userInfo.Email = "No Email Yet"; }


                        userInfo.Role = "MobileUser";
                        userInfo.DeviceID = _deviceId;
                    }


                    if (userInfo != null && userInfo.UserName != null)
                    {
                        tokenString = _securityHelper.GenerateJSONWebToken(userInfo);
                    }

                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = "";
                    _objResponseModel.APIVersion = "0.1";
                    _objResponseModel.Token = tokenString;
                    return Created("Result", _objResponseModel);
                }

                else
                {
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "Refresh Token Expired/Revoked";
                    _objResponseModel.APIVersion = "0.1";
                    _objResponseModel.Token = null;
                    return Created("Result", _objResponseModel);
                }

            }

            #endregion

            else
            {
                _objResponseModel.StatusCode = 1;
                _objResponseModel.ErrorDescription = "Refresh Token Expired/Revoked";
                _objResponseModel.APIVersion = "0.1";
                _objResponseModel.Token = null;
                return Created("Result", _objResponseModel);
            }

           
        }







        private async Task<RefreshToken> Get_RefreshTokenAsync(string _MobileNo, string _Password)
        {
            
              RefreshToken res = new RefreshToken();
              var user1 = new ApplicationUser() { UserName = _MobileNo };
              var user = await _userManager.FindByNameAsync(user1.UserName);
              var Ex_token =await LoginRepository.EPS_Service_Check_Refresh_Token(user.Id);

              if (Ex_token == null)
              {
                var refreshToken = CreateRefreshToken();
                user.RefreshTokens = new List<RefreshToken>();
                user.RefreshTokens.Add(refreshToken);
                _context.Update(user);
                _context.SaveChanges();
                return refreshToken;

              }

            if (Ex_token.IsActive)
            {
                return Ex_token;
            }          
            else
            {
                    var refreshToken = CreateRefreshToken();
                    user.RefreshTokens.Add(refreshToken);
                    _context.Update(user);
                    _context.SaveChanges();
                    return refreshToken;
            }
                    
        }






        #region Refresh Token Helper Method

        private RefreshToken CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var generator = new RNGCryptoServiceProvider())
            {
                generator.GetBytes(randomNumber);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomNumber),
                    Expires = DateTime.UtcNow.AddDays(10),
                    Created = DateTime.UtcNow
                };
            }
        }


        private void SetRefreshTokenInCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(10),
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }


        private void SetMobileNumberInCookie(string _mobilenumber)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(10),
            };
            Response.Cookies.Append("mobileNumber", _mobilenumber, cookieOptions);
        }

        private void SetDeviceIDInCookie(string _mobilenumber)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(10),
            };
            Response.Cookies.Append("deviceId", _mobilenumber, cookieOptions);
        }



        private async Task<int> RevokeRefreshToken(string _MobileNo) 
        {

            var user1 = new ApplicationUser() { UserName = _MobileNo };
            var user = await _userManager.FindByNameAsync(user1.UserName);
            var Ex_token = await LoginRepository.EPS_Service_Check_Refresh_Token(user.Id);

            if (Ex_token != null)
            {
                var revoke= await LoginRepository.EPS_Service_Revoke_Refresh_Token(Ex_token.ID);
                return 1;
            }

            return 0;
        }



        #endregion








    }
}
