using EPS_Service_API.Repositories;
using EPS_Service_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPS_Service_API.API.Repositories;
using EPS_Service_API.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EPS_Service_API.API.V1.Controllers
{
    [ApiVersion("1.0")]
  //  [Route("api/v{version:apiVersion}/[controller]")]

    [ApiController]
    public class TransactionAccountsOfCustomerController : ControllerBase
    {

        private SecurityHelper _securityHelper;
        private ITransactionAccountsOfCustomerRepository TransactionAccountsOfCustomerRepository;
        private readonly ILogger<TransactionAccountsOfCustomerController> _logger;
        private DeviceValidator _DeviceValidator;

        public TransactionAccountsOfCustomerController(
            ITransactionAccountsOfCustomerRepository TransactionAccountsOfCustomerRepository,
            SecurityHelper securityHelper,
            ILogger<TransactionAccountsOfCustomerController> logger,
             DeviceValidator DeviceValidator
            )
        {
            this.TransactionAccountsOfCustomerRepository = TransactionAccountsOfCustomerRepository;
            _securityHelper = securityHelper;
            _logger = logger;
            _DeviceValidator = DeviceValidator;
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "MobileUser")]
        [Route("~/api/v{version:apiVersion}/Accounts/AccountList")]
        [HttpPost]
        public async Task<IActionResult> AccountList_GetByCusID([FromBody] GetAccountDetailEntity GADE)
        {
            string hash = Request.Headers["x-hash"].ToString(); // Check if the hash is valid

            objTxnAcc _objResponseModel = new objTxnAcc();

            #region Device ID validation

            string token = Request.Headers["Authorization"];

            var valid_device = _DeviceValidator.Validator(token, GADE.DeviceID);
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
                var result = await TransactionAccountsOfCustomerRepository.AccountList_GetByCusID(GADE.CusId);

                if (result.Count == 0)
                {
                    _objResponseModel.TxnAcc = null;
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = "Successful Data Load- No Data found";
                    _objResponseModel.APIVersion = "0.1";
                    //   return NotFound( _objResponseModel);
                    return Created("Result", _objResponseModel);

                }
                else
                {
                    _objResponseModel.TxnAcc = result;
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = null;
                    _objResponseModel.APIVersion = "0.1";

                    // API Related Information  start

                    // Method mongodb

                    // API Related Information  start

                    //    return Ok(_objResponseModel);
                    return Created("Result", _objResponseModel);


                }
             
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "We caught this exception in AccountList API.");
                string errorDescription_ = ex.Message.ToString();
                _objResponseModel.TxnAcc = null;
                _objResponseModel.StatusCode = 1;
                // _objResponseModel.ErrorDescription = errorDescription_;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                _objResponseModel.APIVersion = "0.1";
                //    return BadRequest( _objResponseModel);
                return Created("Result", _objResponseModel);
            }


        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "MobileUser")]
        [Route("~/api/v{version:apiVersion}/Accounts/AccountActivation")]
        [HttpPost]
        public async Task<IActionResult> AccountsActiveInactive_byCustomer([FromBody] AccountStatus_UpdateModel ASU)
        {
            objStatusOfOperation _objResponseModel = new objStatusOfOperation();

            #region Device ID validation

            string token = Request.Headers["Authorization"];

            var valid_device = _DeviceValidator.Validator(token, ASU.DeviceID);
            if (!valid_device)
            {
                _objResponseModel.StatusCode = 401;
                _objResponseModel.ErrorDescription = "Device Error!";
                _objResponseModel.APIVersion = "0.1";

                //    return Unauthorized(_objResponseModel);
                return Created("Result", _objResponseModel);
            }

            #endregion

            if (ASU.isActive == false)
            {
                ASU.CurrentStatus = 0;
            }
            if (ASU.isActive == true)
            {
                ASU.CurrentStatus = 1;
            }

            try
            {
                string hash = Request.Headers["x-hash"].ToString(); // Check if the hash is valid
              
                var updatedStatus = await TransactionAccountsOfCustomerRepository.AccountsActiveInactive_byCustomer(ASU);

                if (updatedStatus == "Disabled")
                {
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = $"Account Successfully Disabled";
                    _objResponseModel.APIVersion = "0.1";

                    // API Related Information  start

                    // Method mongodb

                    // API Related Information  start

                    //   return Ok( _objResponseModel);
                    return Created("Result", _objResponseModel);
                }

                else if(updatedStatus == "Enabled")
                {
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = $"Account Successfully Enabled";
                    _objResponseModel.APIVersion = "0.1";

                    // API Related Information  start

                    // Method mongodb

                    // API Related Information  start

                    //     return Ok(_objResponseModel);
                    return Created("Result", _objResponseModel);
                }

                else
                {
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = $"Failed to Change Status!";
                    _objResponseModel.APIVersion = "0.1";
                }

                //   return BadRequest( _objResponseModel);
                return Created("Result", _objResponseModel);



            }


            catch (Exception ex)
            {
                _logger.LogError(ex, "We caught this exception in AccountsActiveInactive_byCustomer API.");
                string errorDescription_ = ex.Message.ToString();
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
        [Route("~/api/v{version:apiVersion}/Accounts/AccountStatusList")]
        [HttpPost]
        public async Task<IActionResult> AccountStatus_AccountList_GetByCusID([FromBody] GetAccountDetailEntity GADE)
        {
            string hash = Request.Headers["x-hash"].ToString(); // Check if the hash is valid

            objAccStat _objResponseModel = new objAccStat();

            #region Device ID validation

            string token = Request.Headers["Authorization"];

            var valid_device = _DeviceValidator.Validator(token, GADE.DeviceID);
            if (!valid_device)
            {
                _objResponseModel.StatusCode = 401;
                _objResponseModel.ErrorDescription = "Device Error!";
                _objResponseModel.APIVersion = "0.1";

                //  return Unauthorized(_objResponseModel);
                return Created("Result", _objResponseModel);
            }

            #endregion

            try
            {
                var result = await TransactionAccountsOfCustomerRepository.AccountStatus_AccountList_Get(GADE.CusId);

                if (result.Count == 0)
                {
                    _objResponseModel.TxnAccStat = null;
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "Successful Data Load- No Data found";
                    _objResponseModel.APIVersion = "0.1";
                    //      return NotFound( _objResponseModel);
                    return Created("Result", _objResponseModel);
                }
                else
                {
                    _objResponseModel.TxnAccStat = result;
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = null;
                    _objResponseModel.APIVersion = "0.1";
                    //     return Ok(_objResponseModel);
                    return Created("Result", _objResponseModel);
                }
         
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "We caught this exception in AccountStatusList API.");
                string errorDescription_ = ex.Message.ToString();
                _objResponseModel.TxnAccStat = null;
                _objResponseModel.StatusCode = 1;
                // _objResponseModel.ErrorDescription = errorDescription_;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                _objResponseModel.APIVersion = "0.1";
                //  return BadRequest( _objResponseModel);
                return Created("Result", _objResponseModel);
            }


        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "MobileUser")]
        [Route("~/api/v{version:apiVersion}/Accounts/AccountSetDefault")]
        [HttpPost]
        public async Task<IActionResult> AccountsDefaultSelection_byCustomer([FromBody] AccountDefault_UpdateModel ASU)
        {
            objStatusOfOperation _objResponseModel = new objStatusOfOperation();

            #region Device ID validation

            string token = Request.Headers["Authorization"];

            var valid_device = _DeviceValidator.Validator(token, ASU.DeviceID);
            if (!valid_device)
            {
                _objResponseModel.StatusCode = 401;
                _objResponseModel.ErrorDescription = "Device Error!";
                _objResponseModel.APIVersion = "0.1";

                //  return Unauthorized(_objResponseModel);
                return Created("Result", _objResponseModel);
            }

            #endregion

            if (ASU.isDefaultCreditAccount == false)
            {
                ASU.CurrentStatus = 0;
            }
            if (ASU.isDefaultCreditAccount == true)
            {
                ASU.CurrentStatus = 1;
            }

            try
            {
                string hash = Request.Headers["x-hash"].ToString(); // Check if the hash is valid

                if (ASU.isActive==false)
                {
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "Selected Account is not Active.";
                    _objResponseModel.APIVersion = "0.1";
                    //   return BadRequest( _objResponseModel);
                    return Created("Result", _objResponseModel);
                }


                var updatedStatus = await TransactionAccountsOfCustomerRepository.AccountsDefaultSelection_byCustomer(ASU);

                if (updatedStatus == "Default")
                {
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = $"Account set as Default";
                    _objResponseModel.APIVersion = "0.1";
                    // API Related Information  start

                    // Method mongodb

                    // API Related Information  start

                    //     return Ok(_objResponseModel);
                    return Created("Result", _objResponseModel);
                }

                else if (updatedStatus == "RemovedDefault")
                {
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = $"Account Removed from Default";
                    _objResponseModel.APIVersion = "0.1";
                    // API Related Information  start

                    // Method mongodb

                    // API Related Information  start

                    //    return Ok(_objResponseModel);
                    return Created("Result", _objResponseModel);
                }

                else
                {
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = $"Failed to Change Status!";
                    _objResponseModel.APIVersion = "0.1";
                }

                //  return BadRequest( _objResponseModel);
                return Created("Result", _objResponseModel);



            }


            catch (Exception ex)
            {
                _logger.LogError(ex, "We caught this exception in AccountSetDefault API.");
                string errorDescription_ = ex.Message.ToString();
                _objResponseModel.StatusCode = 1;
                // _objResponseModel.ErrorDescription = errorDescription_;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                _objResponseModel.APIVersion = "0.1";
                //  return BadRequest(_objResponseModel);
                return Created("Result", _objResponseModel);
            }
        }


    }
}
