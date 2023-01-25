using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EPS_Service_API.Repositories;
using EPS_Service_API.Model;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EPS_Service_API.Utility;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.Net.Http;
using System.Net;
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;
using EPS_Service_API.API.BankServices;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.RegularExpressions;
using EPS_Service_API.API.Data;
using EPS_Service_API.API.Repositories;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using EPS_Service_API.API.Repositories.Notification;
using static System.Net.WebRequestMethods;
using System.Numerics;

namespace EPS_Service_API.API.Controllers.V1
{
    [ApiVersion("1.0")]

    [ApiController]
    public class UserRegistrationController : ControllerBase
    {

        private SecurityHelper _securityHelper;
        private IBankRepository bankRepository;

        private readonly ICreditService creditService;
        private readonly IDebitService debitService;

        private readonly ILogger<UserRegistrationController> _logger;

        private ICustomerProfileRepository _CustomerProfileRepository;

        //Identity Requirement Start
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //Identity Requirement End

        // private readonly EPSDBContext db;

        private readonly IConfiguration _config;
        private readonly HashCreatorValidator _HashCreatorValidator;

        private ISMS_Service _SMS_Service;

        public UserRegistrationController(

            IBankRepository bankRepository,
            SecurityHelper securityHelper,
            ILogger<UserRegistrationController> logger,
           ICustomerProfileRepository CustomerProfileRepository,

            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration config,
            HashCreatorValidator HashCreatorValidator,
            ISMS_Service SMS_Service

            )
        {
            this.bankRepository = bankRepository;
            _securityHelper = securityHelper;
            _logger = logger;
            _CustomerProfileRepository = CustomerProfileRepository;


            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
            _HashCreatorValidator = HashCreatorValidator;
            _SMS_Service = SMS_Service;

        }


        [Route("~/api/v{version:apiVersion}/UserRegistration/SignUp")]
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] RegisterViewModel RVM)
        {
           
            ObjRegistration _objResponseModel = new ObjRegistration();
         
            try
            {


                #region Create User

                RVM.PhoneNo = FixPhoneNo(RVM.PhoneNo);

                var Delete_Incomplete_Registration = await _CustomerProfileRepository.EPS_Service_Delete_IncompleteCustomerSU(RVM.PhoneNo);

                if (IsValidPinNumber(RVM.PIN, RVM.PhoneNo))
                {

                    var user = new ApplicationUser
                    {
                        UserName = RVM.PhoneNo,
                        Email = string.Empty,
                        PhoneNumber = RVM.PhoneNo,
                        PhoneNumberConfirmed = true
                    };

                    var result = await _userManager.CreateAsync(user, RVM.PIN);

                    if (result.Succeeded)
                    {
                        string actionMessage = "User has registered";
                   
                        await _userManager.SetTwoFactorEnabledAsync(user, false);

                        var signinResult = await _signInManager.PasswordSignInAsync(user, RVM.PIN, false, false);

                        if (signinResult.Succeeded)
                        {
                            _logger.LogError("Signin Success without two factor! ASP.NET Identity has gone mad!");

                            #region ByPass2Factor

                            if (!await _roleManager.RoleExistsAsync(UserRoles.MobileUser))
                            {
                                var roleResult = await _roleManager.CreateAsync(new IdentityRole(UserRoles.MobileUser));
                                if (roleResult.Succeeded)
                                {
                                    _logger.LogInformation("Mobile user role created.");
                                }
                                else
                                {
                                    throw new Exception("Mobile user role creation failed.");
                                }
                            }

                            await _userManager.AddToRoleAsync(user, UserRoles.MobileUser);


                            var CreateOTP = await _CustomerProfileRepository.EPS_Service_OTP_Generation(RVM.PhoneNo);

                            string description_res = result.Errors.Select(x => x.Description).ToString();

                            _objResponseModel.Message = "Please verify OTP to complete registration.";
                            _objResponseModel.StatusCode = 200;
                            _objResponseModel.ErrorDescription = null;
                            _objResponseModel.APIVersion = "0.1";
                            _objResponseModel.MobileNumber = RVM.PhoneNo;


                            #region Send SMS Notification
                            string Messgae_body = "Dear Sir," + CreateOTP + " is your One Time Password(OTP). Please validate in 300 Seconds.Helpline: 09614770066.Thank you";
                            await _SMS_Service.SendSMSNotification(RVM.PhoneNo, Messgae_body);
                            #endregion


                            // API Related Information  start

                            // Method mongodb

                            // API Related Information  start

                            //    return Ok( _objResponseModel);
                            return Created("Result", _objResponseModel);

                            #endregion

                        }
                        else if (signinResult.RequiresTwoFactor)
                        {
                            if (!await _roleManager.RoleExistsAsync(UserRoles.MobileUser))
                            {
                                var roleResult = await _roleManager.CreateAsync(new IdentityRole(UserRoles.MobileUser));
                                if (roleResult.Succeeded)
                                {
                                    _logger.LogInformation("Mobile user role created.");
                                }
                                else
                                {
                                    throw new Exception("Mobile user role creation failed.");
                                }
                            }

                            await _userManager.AddToRoleAsync(user, UserRoles.MobileUser);

                            
                            var CreateOTP =await _CustomerProfileRepository.EPS_Service_OTP_Generation(RVM.PhoneNo);

                            string description_res = result.Errors.Select(x => x.Description).ToString();

                            _objResponseModel.Message = "Please verify OTP to complete registration.";
                            _objResponseModel.StatusCode = 200;
                            _objResponseModel.ErrorDescription = null;
                            _objResponseModel.APIVersion = "0.1";
                            _objResponseModel.MobileNumber = RVM.PhoneNo;


                            #region Send SMS Notification
                            string Messgae_body = "Dear Sir," + CreateOTP + " is your One Time Password(OTP). Please validate in 300 Seconds.Helpline: 09614770066.Thank you";
                            await _SMS_Service.SendSMSNotification(RVM.PhoneNo, Messgae_body);
                            #endregion



                            // API Related Information  start

                            // Method mongodb

                            // API Related Information  start

                        //    return Ok( _objResponseModel);

                            return Created("Result", _objResponseModel);

                        }
                        else
                        {
                            _logger.LogInformation("Role Creation Failed.");
                        }


                       




                    }

                    else
                    {

                        
                        var v = result.Errors.Where(x => x.Code.Contains("DuplicateUserName"));
                        if (v != null)
                        {

                            #region Return JSON

                            _objResponseModel.Message = "Already Mobile Number Exist";
                            _objResponseModel.StatusCode = 1;
                            _objResponseModel.ErrorDescription = "Already Mobile Number Exist";
                            _objResponseModel.APIVersion = "0.1";
                            _objResponseModel.MobileNumber = RVM.PhoneNo;



                            // API Related Information  start

                            // Method mongodb

                            // API Related Information  start

                            //    return BadRequest( _objResponseModel);
                            return Created("Result", _objResponseModel);

                            #endregion


                        }


                    }



                }

                else
                {
                    _objResponseModel.Message = "Weak PIN Number";
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "Weak PIN Number";
                    _objResponseModel.APIVersion = "0.1";
                    _objResponseModel.MobileNumber = RVM.PhoneNo;
                    //  return BadRequest( _objResponseModel);
                    return Created("Result", _objResponseModel);
                }



              


                        #endregion


                       

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "We caught this exception in BankList API.");
                string errorDescription_ = ex.Message.ToString();
                _objResponseModel.Message = "Something went wrong!Please try later.";
                _objResponseModel.StatusCode = 1;
                // _objResponseModel.ErrorDescription = errorDescription_;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                _objResponseModel.APIVersion = "0.1";
                //   return BadRequest( _objResponseModel);
                return Created("Result", _objResponseModel);
            }


              return BadRequest( null);
           

        }


        [Route("~/api/v{version:apiVersion}/UserRegistration/OTP_Verification")]
        [HttpPost]
        public async Task<IActionResult> OTP_Verification([FromBody] OTP_VerificationModel RVM)
        {
            ObjOTP_Verification _objResponseModel = new ObjOTP_Verification();
            
            var OTP_Verification_Status = await _CustomerProfileRepository.EPS_Service_OTP_Verification(RVM.MobileNumber,RVM.OTP);

            if (OTP_Verification_Status > 0)
            {
                var ProfileCreationConfirm = await _CustomerProfileRepository.EPS_Service_SignUp_Customer(RVM.MobileNumber);
                
                _objResponseModel.Message = "Successfully Registered";
                _objResponseModel.StatusCode = 200;
                _objResponseModel.ErrorDescription = "Successfully Registered";
                _objResponseModel.APIVersion = "0.1";
                _objResponseModel.MobileNumber = RVM.MobileNumber;

                #region Send SMS Notification
                string Messgae_body = "Dear Sir, Your mobile number " + RVM.MobileNumber+ " has been successfully registered to Easy Payment System(EPS). Helpline: 09614770066. Thank you";
                await _SMS_Service.SendSMSNotification(RVM.MobileNumber, Messgae_body);
                #endregion

                return Created("Result", _objResponseModel);

            }

         
                _objResponseModel.Message = "OTP Verification Failed";
                _objResponseModel.StatusCode = 1;
                _objResponseModel.ErrorDescription = "OTP Verification Failed";
                _objResponseModel.APIVersion = "0.1";
                _objResponseModel.MobileNumber = RVM.MobileNumber;
                return Created("Result", _objResponseModel);
        }


        
        [Route("~/api/v{version:apiVersion}/UserRegistration/PIN_Reset_Change")]
        [HttpPost]
        public async Task<IActionResult> PIN_Reset_Change([FromBody] PIN_ChangeModel RVM)
        {
            ObjPasswordReset _objResponseModel = new ObjPasswordReset();

            RVM.MobileNumber = FixPhoneNo(RVM.MobileNumber);

            if (IsValidPinNumber(RVM.New_PIN, RVM.MobileNumber))
            {
                var CreateOTP = await _CustomerProfileRepository.EPS_Service_OTP_Generation(RVM.MobileNumber);

                if (CreateOTP > 0)
                {
                    var Secret_data = JsonConvert.SerializeObject(RVM);
                    var encrypted_Secret_data = Base64Encode(Secret_data);
                    
           
                    _objResponseModel.Message = "Please verify OTP to Reset PIN.";
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = null;
                    _objResponseModel.APIVersion = "0.1";
                    _objResponseModel.SecretKey = encrypted_Secret_data;

                    #region Send SMS Notification
                    //  string Messgae_body = "Dear Sir,"+ CreateOTP + "is your One Time Password(OTP). Please validate in 300 Seconds.Thank you";
                    string Messgae_body = "Dear Sir,"+ CreateOTP + " is your One Time Password(OTP). Please validate in 300 Seconds.Helpline: 09614770066.Thank you";
                    await _SMS_Service.SendSMSNotification(RVM.MobileNumber, Messgae_body);
                    #endregion 

                 //   return Ok( _objResponseModel);
                    return Created("Result", _objResponseModel);

                }

                else
                {
                    _objResponseModel.Message = "Technical Problem Found";
                    _objResponseModel.StatusCode = 1;
                  //  _objResponseModel.ErrorDescription = "Technical Problem Found";
                    // _objResponseModel.ErrorDescription = errorDescription_;
                    _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                    _objResponseModel.APIVersion = "0.1";
                    _objResponseModel.SecretKey = null;
                //    return BadRequest( _objResponseModel);
                    return Created("Result", _objResponseModel);
                }

            }

            else
            {
                _objResponseModel.Message = "Weak PIN Number";
                _objResponseModel.StatusCode = 1;
                _objResponseModel.ErrorDescription = "Weak PIN Number";
                _objResponseModel.APIVersion = "0.1";
                _objResponseModel.SecretKey = null;
             //   return BadRequest( _objResponseModel);
                return Created("Result", _objResponseModel);

            }

               

        }


        [Route("~/api/v{version:apiVersion}/UserRegistration/OTP_Verification_PIN_Change")]
        [HttpPost]
        public async Task<IActionResult> OTP_Verification_PIN_Change([FromBody] PIN_Reset_OTP_Model RVM)
        {
            ObjOTP_Verification _objResponseModel = new ObjOTP_Verification();

            if (RVM.SecretKey.Length > 10)
            {
                var decrypted_secretdata = Base64Decode(RVM.SecretKey);
                var secretdata_obj = JsonConvert.DeserializeObject<PIN_ChangeModel>(decrypted_secretdata);

                var MobileNumber = FixPhoneNo(secretdata_obj.MobileNumber);

                #region OTP_Validation

                var OTP_Verification_Status = await _CustomerProfileRepository.EPS_Service_OTP_Verification(MobileNumber, RVM.OTP);
                if (OTP_Verification_Status > 0)
                {

                }

                else
                {
                    _objResponseModel.Message = "OTP Verification Failed";
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "OTP Verification Failed";
                    _objResponseModel.APIVersion = "0.1";
                    _objResponseModel.MobileNumber = MobileNumber;
                    return Created("Result", _objResponseModel);
                }

                #endregion

                var user = await _userManager.FindByNameAsync(MobileNumber);
                var resultChPIN = await _userManager.ChangePasswordAsync(user, secretdata_obj.Old_PIN, secretdata_obj.New_PIN);

                if (resultChPIN.Succeeded)
                {
                    _objResponseModel.Message = "Successfully Changed PIN";
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = null;
                    _objResponseModel.APIVersion = "0.1";
                    _objResponseModel.MobileNumber = MobileNumber;

                    #region Send SMS Notification
                    string Messgae_body = "Dear Sir, Your PIN has been changed successfully. Helpline: 09614770066. Thank you";
                    await _SMS_Service.SendSMSNotification(MobileNumber, Messgae_body);
                    #endregion

                    //  return Ok( _objResponseModel);
                    return Created("Result", _objResponseModel);
                }
                else
                {
                    _objResponseModel.Message = "Unable to Change PIN";
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "Unable to Change Password";
                    _objResponseModel.APIVersion = "0.1";
                    _objResponseModel.MobileNumber = MobileNumber;

                    //    return BadRequest(_objResponseModel);
                    return Created("Result", _objResponseModel);
                }


            }

            else
            {
                _objResponseModel.Message = "Unable to Change PIN";
                _objResponseModel.StatusCode = 1;
                ///   _objResponseModel.ErrorDescription = "Unable to Change Password. No Secret data Provided!";
                ///    // _objResponseModel.ErrorDescription = errorDescription_;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                _objResponseModel.APIVersion = "0.1";
                _objResponseModel.MobileNumber = null;

                //   return BadRequest( _objResponseModel);
                return Created("Result", _objResponseModel);
            }



        }



        [Route("~/api/v{version:apiVersion}/UserRegistration/OTP_Verification_PIN_Reset")]
        [HttpPost]
        public async Task<IActionResult> OTP_Verification_PIN_Reset([FromBody] PIN_Reset_OTP_Model RVM)
        {
            ObjOTP_Verification _objResponseModel = new ObjOTP_Verification();

            if (RVM.SecretKey.Length > 10)
            {
                var decrypted_secretdata = Base64Decode(RVM.SecretKey);
                var secretdata_obj = JsonConvert.DeserializeObject<PIN_ChangeModel>(decrypted_secretdata);

                var MobileNumber = FixPhoneNo(secretdata_obj.MobileNumber);

                #region OTP_Validation

                var OTP_Verification_Status = await _CustomerProfileRepository.EPS_Service_OTP_Verification(MobileNumber, RVM.OTP);
                if (OTP_Verification_Status > 0)
                { 
                
                }

                else
                {
                    _objResponseModel.Message = "OTP Verification Failed";
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "OTP Verification Failed";
                    _objResponseModel.APIVersion = "0.1";
                    _objResponseModel.MobileNumber = MobileNumber;
                    return Created("Result", _objResponseModel);
                }

                #endregion

                var user = await _userManager.FindByNameAsync(MobileNumber);
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, code, secretdata_obj.New_PIN);

                if (result.Succeeded)
                {
                    _objResponseModel.Message = "Successfully Reset PIN";
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = null;
                    _objResponseModel.APIVersion = "0.1";
                    _objResponseModel.MobileNumber = MobileNumber;

                    #region Send SMS Notification
                    string Messgae_body = "Dear Sir, Your PIN has been reset successfully. Helpline: 09614770066. Thank you";
                    await _SMS_Service.SendSMSNotification(MobileNumber, Messgae_body);
                    #endregion

                    //   return Ok( _objResponseModel);
                    return Created("Result", _objResponseModel);
                }
                else
                {
                    _objResponseModel.Message = "Unable to Change PIN";
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "Unable to Change Password";
                    _objResponseModel.APIVersion = "0.1";
                    _objResponseModel.MobileNumber = MobileNumber;

                    //    return BadRequest( _objResponseModel);
                    return Created("Result", _objResponseModel);
                }


            }

            else
            {
                _objResponseModel.Message = "Unable to Change PIN";
                _objResponseModel.StatusCode = 1;
                //  _objResponseModel.ErrorDescription = "Unable to Change Password No Secret data Provided!";
                // _objResponseModel.ErrorDescription = errorDescription_;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                _objResponseModel.APIVersion = "0.1";
                _objResponseModel.MobileNumber = null;

                //      return BadRequest( _objResponseModel);
                return Created("Result", _objResponseModel);
            }
            


        }


        #region PIN reset manual from customer support

        [Route("~/api/v{version:apiVersion}/UserRegistration/PasswordResetManual")]
        [HttpPost]
        public async Task<IActionResult> PasswordResetManual([FromBody] PasswordResetManual model)
        {
            ObjPasswordResetManual _objResponseModel = new ObjPasswordResetManual();

            if (Request.Headers["x-apikey"] != _config["API_KEY_Collection:OtherApplication"])
            {
                _objResponseModel.StatusCode = 1;
                _objResponseModel.ErrorDescription = "Illegal Attempt!";
                _objResponseModel.APIVersion = "0.1";
                _objResponseModel.Message = "Illegal Attempt!";
                return Unauthorized(_objResponseModel);
            }


            if (!_HashCreatorValidator.IsValidHash(Request.Headers["x-hash"].ToString(), model.MobileNumber, _config["HashKeyCollection:OtherApplication"]))
            {
                _objResponseModel.StatusCode = 1;
                _objResponseModel.ErrorDescription = "Invalid Attempt!";
                _objResponseModel.APIVersion = "0.1";
                _objResponseModel.Message = "Invalid Attempt!";
                return Unauthorized(_objResponseModel);
            }

            try { 

            int New_Pin = RandomNumberGenerator();
            var MobileNumber = FixPhoneNo(model.MobileNumber);

            var user = await _userManager.FindByNameAsync(MobileNumber);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, code, New_Pin.ToString());

            if (result.Succeeded)
            {
                _objResponseModel.Message = "Successfully Reset PIN";
                _objResponseModel.StatusCode = 200;
                _objResponseModel.ErrorDescription = null;
                _objResponseModel.APIVersion = "0.1";

                    #region Send SMS Notification
                    //  string Messgae_body = "Dear Sir, This is your new PIN" + New_Pin + ". Please change it as soon as possible. Thank you";
                    string Messgae_body = "Dear Sir, Your temporary PIN is "+ New_Pin + ". Please Login and changed your PIN as soon as possible. Helpline: 09614770066. Thank you";
                    await _SMS_Service.SendSMSNotification(MobileNumber, Messgae_body);
                    #endregion 

                return Ok( _objResponseModel);
            }
            else
            {
                _objResponseModel.Message = "Unable to Change PIN";
                _objResponseModel.StatusCode = 1;
                _objResponseModel.ErrorDescription = "Unable to Change Password";
                _objResponseModel.APIVersion = "0.1";
                return BadRequest(_objResponseModel);
            }

            }

            catch (Exception ex)
            {
                _objResponseModel.Message = "Unable to Change PIN";
                _objResponseModel.StatusCode = 1;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                _objResponseModel.APIVersion = "0.1";

                return BadRequest( _objResponseModel);
            }




        }

        #endregion


        [HttpGet]
        [Route("~/api/v{version:apiVersion}/Common/TestSMS_Send/{MobileNumber}")]
        public async Task<IActionResult> TestSMS_Send(string MobileNumber)
        {
            #region Send SMS Notification
            string Messgae_body_ = "Dear Sir, This is Test SMS. Thank you";
            await _SMS_Service.SendSMSNotification_Test(MobileNumber, Messgae_body_);
            #endregion
            return Ok("1");
        }



        #region

        private int RandomNumberGenerator()
        {
            Random r = new Random();
            return r.Next(10000,99999);
        }


        private string FixPhoneNo(string phoneNo)
        {
            string output = Regex.Replace(phoneNo, @"[-+\(\)\.]|\s+", "");
            if (output.StartsWith("0"))
            {
                output = "+88" + output;
            }
            if (!output.StartsWith("+"))
            {
                output = '+' + output;
            }
            return output;
        }

        private bool IsValidPinNumber(string pIN, string phoneNumber)
        {
            //validate for consecutive
            var isConsecutive = IsConsecutive(pIN);
            //validate for reverse consecutive
            var isReverseConsecutive = IsConsecutiveReverse(pIN);
            //validate if any 4 digits are same
            var isSame4Digit = IsSameValueFor4Digit(pIN);
            //validate if the number is in Year Pattern
            var isYearPatternNumber = IsYearPattern(pIN);
            //validate if the pin number is the part of the mobile number
            var isPartOfMobileNumber = IsPartOfMobileNumber(pIN, phoneNumber);

            bool isVerified = true;
            if (isConsecutive || isReverseConsecutive || isSame4Digit || isYearPatternNumber || isPartOfMobileNumber)
            {
                isVerified = false;
            }

            return isVerified;
        }

        private bool IsPartOfMobileNumber(string pIN, string phoneNumber)
        {
            return phoneNumber.Contains(pIN);
        }

        /// <summary>
        /// validate for consecutive
        /// </summary>
        /// <param name="pin"></param>
        /// <returns></returns>
        private static bool IsConsecutive(string pin)
        {
            int[] list = pin.ToCharArray().Select(Convert.ToInt32).ToArray();

            var isConsecutive = list.Select((n, index) => n == index + list.ElementAt(0)).All(n => n);
            return isConsecutive;
        }

        /// <summary>
        /// //validate for reverse consecutive
        /// </summary>
        /// <param name="pin"></param>
        /// <returns></returns>
        private static bool IsConsecutiveReverse(string pin)
        {
            int[] list = pin.ToCharArray().Select(Convert.ToInt32).Reverse().ToArray();

            var isConsecutive = list.Select((n, index) => n == index + list.ElementAt(0)).All(n => n);
            return isConsecutive;
        }

        /// <summary>
        /// validate if any 4 digits are same
        /// </summary>
        /// <param name="pin"></param>
        /// <returns></returns>
        private static bool IsSameValueFor4Digit(string pin)
        {
            int[] list = pin.ToCharArray().Select(Convert.ToInt32).ToArray();
            var groups = list.GroupBy(v => v);

            foreach (var group in groups)
            {
                if (group.Count() > 3)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// validate if the number is in Year Pattern
        /// </summary>
        /// <param name="pin"></param>
        /// <returns></returns>
        private static bool IsYearPattern(string pin)
        {
            int pinNum = Int32.Parse(pin);
            if (pinNum >= 19000 && pinNum < 21000)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        #endregion


        private static class UserRoles
        {
            public const string MobileUser = "MobileUser";
            public const string Admin = "Admin";
            public const string Accountant = "Accountant";
            public const string StakeHolder = "StakeHolder";
            public const string OperatorChecker = "OperatorChecker";
            public const string OperatorMaker = "OperatorMaker";
            public const string OperatorAdminChecker = "OperatorAdminChecker";
            public const string OperatorAdminMaker = "OperatorAdminMaker";
            public const string BillViewer = "BillViewer";
        }



        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }


        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }








    }
}
