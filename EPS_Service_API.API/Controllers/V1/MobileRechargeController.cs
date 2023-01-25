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

namespace EPS_Service_API.API.V1.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    public class MobileRechargeController : ControllerBase
    {
        private SecurityHelper _securityHelper;
        private IBankRepository bankRepository;

        private readonly ICreditService creditService;
        private readonly IDebitService debitService;

        private readonly ILogger<BankController> _logger;
        private DeviceValidator _DeviceValidator;

        public MobileRechargeController(
            IBankRepository bankRepository,
            SecurityHelper securityHelper,
            ICreditService _creditService,
            IDebitService _debitService,
            ILogger<BankController> logger,
             DeviceValidator DeviceValidator
            )
        {
            this.bankRepository = bankRepository;
            _securityHelper = securityHelper;
            creditService = _creditService;
            debitService = _debitService;
            _logger = logger;
            _DeviceValidator = DeviceValidator;
        }



        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "MobileUser")]
        [Route("~/api/v{version:apiVersion}/MobileRecharge/Initiate")]
        [HttpPost]
        public async Task<IActionResult> Initiate([FromBody] MobileRechargeInitiate obj_in)
        {
            //if (!_securityHelper.IsValidHash(Request.Headers["x-hash"].ToString(), "GetBankByCusId"))
            //    return Unauthorized();

            objMobileRechargeInitiate _objResponseModel = new objMobileRechargeInitiate();

            #region Device ID validation

            string token = Request.Headers["Authorization"];

            var valid_device = _DeviceValidator.Validator(token, obj_in.DeviceID);
            if (!valid_device)
            {
                _objResponseModel.StatusCode = 401;
                _objResponseModel.ErrorDescription = "Device Error!";
                _objResponseModel.APIVersion = "0.1";

             //   return Unauthorized(_objResponseModel);
                return Created("Result", _objResponseModel);
            }

            #endregion


            try
            {
                var result = await bankRepository.EPS_Service_MobileRechargeEntity_Get();
               
                MobileRechargeChargeInfo amount_in = new MobileRechargeChargeInfo();
                amount_in.Amount = obj_in.Amount;
                amount_in.Charge = 0;
                amount_in.Total = amount_in.Amount+ amount_in.Charge;

                MobileRechargeBenInfo Ben_in = new MobileRechargeBenInfo();
                Ben_in.MobileNumber = obj_in.MobileNumber;
                Ben_in.Name = obj_in.Name;

                if (result == null)
                {
                    _objResponseModel.MobileRechargeChargeInfo = null;
                    _objResponseModel.MobileRechargeEntity = null;
                    _objResponseModel.MobileRechargeBenInfo = null;
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "Successful Data Load- No Data found";
                    _objResponseModel.APIVersion = "0.1";

                  //  return NotFound( _objResponseModel);
                    return Created("Result", _objResponseModel);

                }
                else
                {
                    _objResponseModel.MobileRechargeChargeInfo = amount_in;
                    _objResponseModel.MobileRechargeBenInfo = Ben_in;
                    _objResponseModel.MobileRechargeEntity = result;
                    _objResponseModel.Operator = "Robi";
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = null;
                    _objResponseModel.APIVersion = "0.1";



                    // API Related Information  start

                    // Method mongodb

                    // API Related Information  start

                    //   return Ok(_objResponseModel);
                    return Created("Result", _objResponseModel);

                }
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "We caught this exception in Mobile Recharge Initiate API.");
                string errorDescription_ = ex.Message.ToString();
                _objResponseModel.MobileRechargeChargeInfo = null;
                _objResponseModel.MobileRechargeEntity = null;
                _objResponseModel.MobileRechargeBenInfo = null;
                _objResponseModel.StatusCode = 1;
                // _objResponseModel.ErrorDescription = errorDescription_;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                _objResponseModel.APIVersion = "0.1";
                //   return BadRequest( _objResponseModel);
                return Created("Result", _objResponseModel);
            }
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "MobileUser")]
        [Route("~/api/v{version:apiVersion}/MobileRecharge/Verify")]
        [HttpPost]
        public async Task<IActionResult> Verify([FromBody] MobileRechargeVerify obj_in)
        {
            //if (!_securityHelper.IsValidHash(Request.Headers["x-hash"].ToString(), "GetBankByCusId"))
            //    return Unauthorized();

            objMobileRechargeVerify _objResponseModel = new objMobileRechargeVerify();

            #region Device ID validation

            string token = Request.Headers["Authorization"];

            var valid_device = _DeviceValidator.Validator(token, obj_in.DeviceID);
            if (!valid_device)
            {
                _objResponseModel.StatusCode = 401;
                _objResponseModel.ErrorDescription = "Device Error!";
                _objResponseModel.APIVersion = "0.1";

                //     return Unauthorized(_objResponseModel);
                return Created("Result", _objResponseModel);
            }

            #endregion


            try
            {
                obj_in.DeviceID = "";
                _objResponseModel.MobileRechargeVerify = obj_in;
                _objResponseModel.StatusCode = 200;
                _objResponseModel.ErrorDescription = null;
                _objResponseModel.APIVersion = "0.1";
                return Created("Result", _objResponseModel);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "We caught this exception in Mobile Recharge verify API.");
                string errorDescription_ = ex.Message.ToString();
                _objResponseModel.MobileRechargeVerify = null;
                _objResponseModel.StatusCode = 1;
                // _objResponseModel.ErrorDescription = errorDescription_;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                _objResponseModel.APIVersion = "0.1";
                return Created("Result", _objResponseModel);
            }
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "MobileUser")]
        [Route("~/api/v{version:apiVersion}/MobileRecharge/Confirm")]
        [HttpPost]
        public async Task<IActionResult> Confirm([FromBody] MobileRechargeConfirm obj_in)
        {
            //if (!_securityHelper.IsValidHash(Request.Headers["x-hash"].ToString(), "GetBankByCusId"))
            //    return Unauthorized();

            objMobileRechargeConfirm _objResponseModel = new objMobileRechargeConfirm();

            #region Device ID validation

            string token = Request.Headers["Authorization"];

            var valid_device = _DeviceValidator.Validator(token, obj_in.DeviceID);
            if (!valid_device)
            {
                _objResponseModel.StatusCode = 401;
                _objResponseModel.ErrorDescription = "Device Error!";
                _objResponseModel.APIVersion = "0.1";

                //    return Unauthorized(_objResponseModel);
                return Created("Result", _objResponseModel);
            }

            #endregion


            try
            {
                if (obj_in.OTP != "234567")
                {
                    _objResponseModel.Message = null;
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "OTP Verification Failed!";
                    _objResponseModel.APIVersion = "0.1";
                    //    return NotFound( _objResponseModel);
                    return Created("Result", _objResponseModel);
                }

                else
                {
                    _objResponseModel.Message = "Your Mobile Recharge was Successful!";
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = null;
                    _objResponseModel.APIVersion = "0.1";
                    //    return Ok( _objResponseModel);
                    return Created("Result", _objResponseModel);
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "We caught this exception in Mobile Recharge Confirm API.");
                string errorDescription_ = ex.Message.ToString();
                _objResponseModel.Message = null;
                _objResponseModel.StatusCode = 1;
                // _objResponseModel.ErrorDescription = errorDescription_;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                _objResponseModel.APIVersion = "0.1";
                //  return BadRequest( _objResponseModel);
                return Created("Result", _objResponseModel);
            }
        }





    }
}
