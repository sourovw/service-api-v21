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
   // [Route("api/v{version:apiVersion}/[controller]")]

    [ApiController]
    public class BankController : ControllerBase
    {
        private SecurityHelper _securityHelper;
        private IBankRepository bankRepository;

        private readonly ICreditService creditService;
        private readonly IDebitService debitService;

        private readonly ILogger<BankController> _logger;

        private DeviceValidator _DeviceValidator;

        public BankController(
            IBankRepository bankRepository, 
            SecurityHelper securityHelper,
            DeviceValidator DeviceValidator,
            ICreditService _creditService, 
            IDebitService _debitService,
            ILogger<BankController> logger
            )
        {
            this.bankRepository = bankRepository;
            _securityHelper = securityHelper;
            _DeviceValidator = DeviceValidator;
            creditService = _creditService;
            debitService = _debitService;
            _logger = logger;
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "MobileUser")]
        [Route("~/api/v{version:apiVersion}/Bank/BankList")]
        [HttpPost]
        public async Task<IActionResult> GetBankByCusId([FromBody] GetFinancialentity GetFinancialentity)
        {
            //if (!_securityHelper.IsValidHash(Request.Headers["x-hash"].ToString(), "GetBankByCusId"))
            //    return Unauthorized();

            objBankList _objResponseModel = new objBankList();


            #region Device ID validation

            string token = Request.Headers["Authorization"];

            var valid_device = _DeviceValidator.Validator(token, GetFinancialentity.DeviceID);
            if (!valid_device)
            {
                _objResponseModel.StatusCode = 401;
                _objResponseModel.ErrorDescription = "Device Error!";
                _objResponseModel.APIVersion = "0.1";

                // return Unauthorized(_objResponseModel);
                return Created("Result", _objResponseModel);
            }

            #endregion


            try
            {
                var result = await bankRepository.GetBankByCusId(GetFinancialentity.CusId);
                if (result == null)
                {
                    _objResponseModel.BankList = null;
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "Successful Data Load- No Data found";
                    _objResponseModel.APIVersion = "0.1";
                    //  return NotFound(_objResponseModel);
                    return Created("Result", _objResponseModel);
                }
                else
                {
                    _objResponseModel.BankList = result;
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = null;
                    _objResponseModel.APIVersion = "0.1";



                    // API Related Information  start

                    // Method mongodb

                    // API Related Information  start

                }
                //  return Ok(_objResponseModel);
                return Created("Result", _objResponseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "We caught this exception in BankList API.");
                string errorDescription_ = ex.Message.ToString();
                _objResponseModel.BankList = null;
                _objResponseModel.StatusCode = 1;
                // _objResponseModel.ErrorDescription = errorDescription_;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                _objResponseModel.APIVersion = "0.1";
                //   return BadRequest(_objResponseModel);
                return Created("Result", _objResponseModel);
            }
        }



       
        [HttpPost]
        [Route("~/api/v{version:apiVersion}/Bank/ValidateCnD")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status451UnavailableForLegalReasons)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ValidateCnD([FromBody] CreditTransactionModel inputModel)
        {
            oBjBankValidateCnD _objResponseModel = new oBjBankValidateCnD();

            CreditTransactionModel transaction = await creditService.ValidateCredit(inputModel);
            
            // if Valid Credit Account and Amount
            if (transaction.IsSuccess == true)
            {
                inputModel.TransferId = transaction.TransferId;
                CreditTransactionModel transactionValidateDebit = await debitService.ValidateDebit(inputModel);
                if (transactionValidateDebit.IsSuccess == true)
                {
                    var result = transactionValidateDebit.TransferId;

                    _objResponseModel.APIVersion = "0.1";
                    _objResponseModel.TransferId = Convert.ToString(transactionValidateDebit.TransferId);
                    _objResponseModel.StatusCode = 0;
                    _objResponseModel.ErrorDescription = "Validate Credit and Debit Successfully";
                    _objResponseModel.Bankresult = transactionValidateDebit.Bankresult;
                    //    return Ok(_objResponseModel);
                    return Created("Result", _objResponseModel);
                }
                else
                {
                    _objResponseModel.APIVersion = "0.1";
                    _objResponseModel.TransferId = Convert.ToString(transactionValidateDebit.TransferId);
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = transaction.ErrorDescription;
                    _objResponseModel.Bankresult = transactionValidateDebit.Bankresult;
                    // return BadRequest(_objResponseModel);
                    return Created("Result", _objResponseModel);
                }
            }
            else
            {
                _logger.LogError(transaction.ErrorDescription, "We caught this exception in ValidateCnD API.");
                _objResponseModel.APIVersion = "0.1";
                _objResponseModel.TransferId = "0";
                _objResponseModel.StatusCode = 1;
                _objResponseModel.ErrorDescription = "Validate Credit and Debit not Successfully";
                _objResponseModel.Bankresult = transaction.ErrorDescription;
                //  return BadRequest(_objResponseModel);
                return Created("Result", _objResponseModel);
            }



        }


        #region Bank Routing Information

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "MobileUser")]
        [Route("~/api/v{version:apiVersion}/Bank/ToBankList")]
        [HttpPost]
        public async Task<IActionResult> SA_AllBank_ToSend_Get([FromBody] To_Bank_Branch_Routing_Arg TBBRA)
        {
            //if (!_securityHelper.IsValidHash(Request.Headers["x-hash"].ToString(), "GetBankByCusId"))
            //    return Unauthorized();

            objToBankList _objResponseModel = new objToBankList();

            #region Device ID validation

            string token = Request.Headers["Authorization"];

            var valid_device = _DeviceValidator.Validator(token, TBBRA.DeviceID);
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
                var result = await bankRepository.SA_AllBank_ToSend_Get();

                if (result == null)
                {
                    _objResponseModel.To_BankList = null;
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "Successful Data Load- No Data found";
                    _objResponseModel.APIVersion = "0.1";
                    //    return NotFound( _objResponseModel);
                    return Created("Result", _objResponseModel);
                }
                else
                {
                    _objResponseModel.To_BankList = result;
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = null;
                    _objResponseModel.APIVersion = "0.1";



                    // API Related Information  start

                    // Method mongodb

                    // API Related Information  start

                }
                //   return Ok(_objResponseModel);
                return Created("Result", _objResponseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "We caught this exception in BankList API.");
                string errorDescription_ = ex.Message.ToString();
                _objResponseModel.To_BankList = null;
                _objResponseModel.StatusCode = 1;
                _objResponseModel.ErrorDescription = "Something went wrong! Please try later.";
                _objResponseModel.APIVersion = "0.1";
                //     return BadRequest(_objResponseModel);
                return Created("Result", _objResponseModel);
            }
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "MobileUser")]
        [Route("~/api/v{version:apiVersion}/Bank/ToBankBranchRouting")]
        [HttpPost]
        public async Task<IActionResult> SA_Bank_Routing_ToSend_GetbyBankID([FromBody] To_Bank_Branch_Routing_Arg TBBRA)
        {
            //if (!_securityHelper.IsValidHash(Request.Headers["x-hash"].ToString(), "GetBankByCusId"))
            //    return Unauthorized();

            objToBankRouting _objResponseModel = new objToBankRouting();

            #region Device ID validation

            string token = Request.Headers["Authorization"];

            var valid_device = _DeviceValidator.Validator(token, TBBRA.DeviceID);
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
                var result = await bankRepository.SA_Bank_Routing_ToSend_GetbyBankID(TBBRA.BankID);

                if (result == null)
                {
                    _objResponseModel.To_Bank_Branch_Routing = null;
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "Successful Data Load- No Data found";
                    _objResponseModel.APIVersion = "0.1";
                    //     return NotFound(_objResponseModel);
                    return Created("Result", _objResponseModel);
                }
                else
                {
                    _objResponseModel.To_Bank_Branch_Routing = result;
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = null;
                    _objResponseModel.APIVersion = "0.1";



                    // API Related Information  start

                    // Method mongodb

                    // API Related Information  start

                }
                //  return Ok(_objResponseModel);
                return Created("Result", _objResponseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "We caught this exception in BankList API.");
                string errorDescription_ = ex.Message.ToString();
                _objResponseModel.To_Bank_Branch_Routing = null;
                _objResponseModel.StatusCode = 1;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                _objResponseModel.APIVersion = "0.1";
                //     return BadRequest(_objResponseModel);
                return Created("Result", _objResponseModel);
            }
        }

        #endregion


        #region Send Money Using Mobile

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "MobileUser")]
        [Route("~/api/v{version:apiVersion}/Bank/BankListbyMoNo")]
        [HttpPost]
        public async Task<IActionResult> BA_CustomeraccountByMobileNumber([FromBody] To_MobileNumber_List_Arg TMN)
        {
            //if (!_securityHelper.IsValidHash(Request.Headers["x-hash"].ToString(), "GetBankByCusId"))
            //    return Unauthorized();

            objToBankRouting _objResponseModel = new objToBankRouting();

            #region Device ID validation

            string token = Request.Headers["Authorization"];

            var valid_device = _DeviceValidator.Validator(token, TMN.DeviceID);
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
                var result = await bankRepository.BA_CustomeraccountByMobileNumber(_DeviceValidator.FixPhoneNo(TMN.MobileNumber));

                if (result == null)
                {
                    _objResponseModel.To_Bank_Branch_Routing = null;
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "Successful Data Load- No Data found";
                    _objResponseModel.APIVersion = "0.1";
                    //      return NotFound(_objResponseModel);
                    return Created("Result", _objResponseModel);
                }
                else
                {
                    _objResponseModel.To_Bank_Branch_Routing = result;
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = null;
                    _objResponseModel.APIVersion = "0.1";



                    // API Related Information  start

                    // Method mongodb

                    // API Related Information  start

                }
                //    return Ok( _objResponseModel);
                return Created("Result", _objResponseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "We caught this exception in BankList API.");
                string errorDescription_ = ex.Message.ToString();
                _objResponseModel.To_Bank_Branch_Routing = null;
                _objResponseModel.StatusCode = 1;
                // _objResponseModel.ErrorDescription = errorDescription_;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                _objResponseModel.APIVersion = "0.1";
                //    return BadRequest( _objResponseModel);
                return Created("Result", _objResponseModel);
            }
        }


        #endregion





      

    }
}

