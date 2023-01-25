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
using Microsoft.Extensions.Configuration;

namespace EPS_Service_API.API.V1.Controllers
{
    [ApiVersion("1.0")]
 
    [ApiController]
    public class TransactionDetailController : ControllerBase
    {
        private SecurityHelper _securityHelper;
        private ITransactionDetailRepository transactionDetailRepository;
        private readonly ILogger<TransactionDetailController> _logger;
        private readonly IConfiguration _config;
        private DeviceValidator _DeviceValidator;

        public TransactionDetailController(
            ITransactionDetailRepository transactionDetailRepository, 
            SecurityHelper securityHelper,
            ILogger<TransactionDetailController> logger,
            IConfiguration config,
            DeviceValidator DeviceValidator
            )
        {
            this.transactionDetailRepository = transactionDetailRepository;
            _securityHelper = securityHelper;
            _logger = logger;
            _config = config;
            _DeviceValidator = DeviceValidator;
        }


        [Route("~/api/v{version:apiVersion}/Transactions/TransactionList")]
        [HttpPost]
        public async Task<IActionResult> TransactionListGet_byCusID([FromBody] GetTxnDetailListEntity GTDLE)
        {
            string hash = Request.Headers["x-hash"].ToString(); // Check if the hash is valid

            objTransactionDetail _objResponseModel = new objTransactionDetail();

            #region Device ID validation

            string token = Request.Headers["Authorization"];

            var valid_device = _DeviceValidator.Validator(token, GTDLE.DeviceID);
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
                var result_Rec_Money = await transactionDetailRepository.Transaction_ReceiveMoneyList_GetByID(GTDLE);
                var result_Send_Money = await transactionDetailRepository.Transaction_SendMoneyList_GetByID(GTDLE);

                if (result_Rec_Money.Count == 0 && result_Send_Money.Count == 0)
                {
                    _objResponseModel.ReceiveMoneyList = null;
                    _objResponseModel.SendMoneyList = null;

                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "Successful Data Load- No Data found";
                    _objResponseModel.APIVersion = "0.1";
                    //      return NotFound( _objResponseModel);
                    return Created("Result", _objResponseModel);
                }
                else
                {
                    if (result_Rec_Money.Count == 0)
                    {
                    _objResponseModel.ReceiveMoneyList = null;
                    }
                    else
                    {
                    _objResponseModel.ReceiveMoneyList = result_Rec_Money;
                    }


                    if (result_Send_Money.Count == 0)
                    {
                        _objResponseModel.SendMoneyList = null;
                    }
                    else
                    {
                        _objResponseModel.SendMoneyList = result_Send_Money;
                    }

                   

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
                _logger.LogError(ex, "We caught this exception in TransactionList API.");
                string errorDescription_ = ex.Message.ToString();
                _objResponseModel.ReceiveMoneyList = null;
                _objResponseModel.SendMoneyList = null;
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
        [Route("~/api/v{version:apiVersion}/Transactions/TransactionStatusList")]
        [HttpPost]
        public async Task<IActionResult> TransactionStatusListGet_byCusID([FromBody] GetTxnDetailListEntity GTDLE)
        {
            string hash = Request.Headers["x-hash"].ToString(); // Check if the hash is valid

            objTxnStatList _objResponseModel = new objTxnStatList();

            #region Device ID validation

            string token = Request.Headers["Authorization"];

            var valid_device = _DeviceValidator.Validator(token, GTDLE.DeviceID);
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
                var TransactionStatusTxnList = await transactionDetailRepository.TransactionStatusListGet_byCusID(GTDLE);

                if (TransactionStatusTxnList.Count == 0)
                {
                    _objResponseModel.TxnStatList = null;
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "Successful Data Load- No Data found";
                    _objResponseModel.APIVersion = "0.1";

                    //    return NotFound(_objResponseModel);
                    return Created("Result", _objResponseModel);
                }
                else
                {
                    _objResponseModel.TxnStatList = TransactionStatusTxnList;
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
                _logger.LogError(ex, "We caught this exception in TransactionStatusList API.");
                string errorDescription_ = ex.Message.ToString();
                _objResponseModel.TxnStatList = null;
                _objResponseModel.StatusCode = 1;
                // _objResponseModel.ErrorDescription = errorDescription_;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                _objResponseModel.APIVersion = "0.1";

                //    return BadRequest(_objResponseModel);
                return Created("Result", _objResponseModel);
            }


        }


     
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "MobileUser")]
        [Route("~/api/v{version:apiVersion}/Transactions/TransactionDetail")]
        [HttpPost]
        public async Task<IActionResult> TransactionDetailGet_byTxnID([FromBody] GetTxnEntity GTDE)
        {

            string hash = Request.Headers["x-hash"].ToString(); // Check if the hash is valid

            objTxnStat _objResponseModel = new objTxnStat();
            objTrxbByID obj_ret = new objTrxbByID();

            #region Device ID validation

            string token = Request.Headers["Authorization"];

            var valid_device = _DeviceValidator.Validator(token, GTDE.DeviceID);
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
                var db_obj = await transactionDetailRepository.TransactionDetailGet_byTxnID(GTDE.TxnId);

                if (db_obj == null)
                {
                    //  _objResponseModel.TxnStat = TransactionStatusTxnDetail;
                    obj_ret.StatusCode = 1;
                    obj_ret.ErrorDescription = "Successful Data Load- No Data found";
                    obj_ret.APIVersion = "0.1";
                    //  return BadRequest(_objResponseModel);
                    return Created("Result", _objResponseModel);

                }
                else
                {
                    #region DataBind



                    obj_ret.Amount = db_obj.Amount.ToString();
                    obj_ret.ChargeAmount = db_obj.SendingCharge.ToString();
                    obj_ret.VatAmount = db_obj.Vat.ToString();

                    obj_ret.ToAccountId = db_obj.ToAccount.ToString();
                    obj_ret.Bankresult = "";
                    obj_ret.TransactionId = db_obj.TransferID.ToString();
                    obj_ret.TransferId = db_obj.TransferID.ToString();

                    obj_ret.TransfarDate = db_obj.TransfarDate.ToString();
                    obj_ret.TransfarTime = db_obj.TransfarTime.ToString();

                    obj_ret.TransactionNote = db_obj.Remark.ToString();
                    obj_ret.TransactionReference= db_obj.Reference.ToString();

                    obj_ret.APIVersion = "0.1";
                    obj_ret.ErrorDescription = "";
                    obj_ret.StatusCode = 200;


                    obj_ret.ToAccountDetails = await transactionDetailRepository.GetDetailsByToAccountId(Convert.ToInt32(db_obj.ToAccount));
                    obj_ret.FromAccountDetails = await transactionDetailRepository.GetDetailsByFromAccountId(Convert.ToInt32(db_obj.FromAccount));


                    #endregion

                    //  _objResponseModel.TxnStat = TransactionStatusTxnDetail;
                    obj_ret.StatusCode = 200;
                    obj_ret.ErrorDescription = null;
                    obj_ret.APIVersion = "0.1";

                    // API Related Information  start

                    // Method mongodb

                    // API Related Information  start

                    //   return Ok(_objResponseModel);
                    return Created("Result", obj_ret);
                }
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "We caught this exception in TransactionDetail API.");
                string errorDescription_ = ex.Message.ToString();
                obj_ret.StatusCode = 1;
                // _objResponseModel.ErrorDescription = errorDescription_;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                obj_ret.APIVersion = "0.1";
                //  return BadRequest( obj_ret);
                return Created("Result", _objResponseModel);
            }


        }



        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "MobileUser")]
        [Route("~/api/v{version:apiVersion}/Transactions/HistoryProfile")]
        [HttpPost]
        public async Task<IActionResult> Transaction_PreviousProfile_GetByID([FromBody] GetTxnDetailListEntity GTDLE)
        {
            string hash = Request.Headers["x-hash"].ToString(); // Check if the hash is valid

            objHistoryContact _objResponseModel = new objHistoryContact();

            #region Device ID validation

            string token = Request.Headers["Authorization"];

            var valid_device = _DeviceValidator.Validator(token, GTDLE.DeviceID);
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
                var UsedAccountList = await transactionDetailRepository.Transaction_PreviousProfile_GetByID(GTDLE);

                if (UsedAccountList.Count == 0)
                {
                    _objResponseModel.UsedAccounts = null;
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "Successful Data Load- No Data found";
                    _objResponseModel.APIVersion = "0.1";
                    //   return NotFound(_objResponseModel);
                    return Created("Result", _objResponseModel);

                }
                else
                {
                    _objResponseModel.UsedAccounts = UsedAccountList;
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = null;
                    _objResponseModel.APIVersion = "0.1";
                    // API Related Information  start

                    // Method mongodb

                    // API Related Information  start

                    //     return Ok(_objResponseModel);
                    return Created("Result", _objResponseModel);
                }
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "We caught this exception in TransactionStatusList API.");
                string errorDescription_ = ex.Message.ToString();
                _objResponseModel.UsedAccounts = null;
                _objResponseModel.StatusCode = 1;
                // _objResponseModel.ErrorDescription = errorDescription_;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                _objResponseModel.APIVersion = "0.1";
                //     return BadRequest( _objResponseModel);
                return Created("Result", _objResponseModel);
            }


        }



        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "MobileUser")]
        [Route("~/api/v{version:apiVersion}/Transactions/TransactionList_Send")]
        [HttpPost]
        public async Task<IActionResult> TransactionListSendGet_byCusID([FromBody] GetTxnDetailListEntity GTDLE)
        {
            string hash = Request.Headers["x-hash"].ToString(); // Check if the hash is valid

            objTransactionDetail_Send _objResponseModel = new objTransactionDetail_Send();

            #region Device ID validation

            string token = Request.Headers["Authorization"];

            var valid_device = _DeviceValidator.Validator(token, GTDLE.DeviceID);
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
                var result_Send_Money = await transactionDetailRepository.Transaction_SendMoneyList_GetByID(GTDLE);

                if (result_Send_Money.Count == 0)
                {
                    
                    _objResponseModel.SendMoneyList = null;

                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "Successful Data Load- No Data found";
                    _objResponseModel.APIVersion = "0.1";

                    //   return NotFound(_objResponseModel);
                    return Created("Result", _objResponseModel);
                }
                else
                {
                  

                    if (result_Send_Money.Count == 0)
                    {
                        _objResponseModel.SendMoneyList = null;
                    }
                    else
                    {
                        _objResponseModel.SendMoneyList = result_Send_Money;
                    }



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
                _logger.LogError(ex, "We caught this exception in TransactionList API.");
                string errorDescription_ = ex.Message.ToString();
                _objResponseModel.SendMoneyList = null;
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
        [Route("~/api/v{version:apiVersion}/Transactions/TransactionList_Received")]
        [HttpPost]
        public async Task<IActionResult> TransactionList_ReceivedGet_byCusID([FromBody] GetTxnDetailListEntity GTDLE)
        {
            string hash = Request.Headers["x-hash"].ToString(); // Check if the hash is valid

            objTransactionDetail_Received _objResponseModel = new objTransactionDetail_Received();

            #region Device ID validation

            string token = Request.Headers["Authorization"];

            var valid_device = _DeviceValidator.Validator(token, GTDLE.DeviceID);
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
                var result_Rec_Money = await transactionDetailRepository.Transaction_ReceiveMoneyList_GetByID(GTDLE);
             
                if (result_Rec_Money.Count == 0 )
                {
                    _objResponseModel.ReceiveMoneyList = null;
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "Successful Data Load- No Data found";
                    _objResponseModel.APIVersion = "0.1";
                    //    return NotFound(_objResponseModel);
                    return Created("Result", _objResponseModel);

                }
                else
                {
                    if (result_Rec_Money.Count == 0)
                    {
                        _objResponseModel.ReceiveMoneyList = null;
                    }
                    else
                    {
                        _objResponseModel.ReceiveMoneyList = result_Rec_Money;
                    }



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
                _logger.LogError(ex, "We caught this exception in TransactionList API.");
                string errorDescription_ = ex.Message.ToString();
                _objResponseModel.ReceiveMoneyList = null;
                _objResponseModel.StatusCode = 1;
                // _objResponseModel.ErrorDescription = errorDescription_;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                _objResponseModel.APIVersion = "0.1";
                //   return BadRequest( _objResponseModel);
                return Created("Result", _objResponseModel);
            }


        }



        [HttpPost]
        [Route("~/api/v{version:apiVersion}/Transactions/PG_PaymentTransactionPurchase_Set")]
        public async Task<IActionResult> PG_PaymentTransactionPurchase_Set([FromBody] PG_PaymentTransactionUser model)
        {
            objPG_PaymentTransactionUser _objResponseModel = new objPG_PaymentTransactionUser();

            string API_Key = Request.Headers["x-apikey"].ToString();

            if (API_Key != _config["API_KEY_Collection:OtherApplication"])
            {
                _objResponseModel.StatusCode = 1;
                _objResponseModel.ErrorDescription = "Illegal Access Request";
                _objResponseModel.APIVersion = "0.1";

                   return Unauthorized(_objResponseModel);
             //   return Created("Result", _objResponseModel);
            }

            try
            {
                var result = await transactionDetailRepository.PG_PaymentTransactionPurchase_Set(model);

                if (result < 1)
                {
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "An unexpected error happen!";
                    _objResponseModel.APIVersion = "0.1";
                       return BadRequest( _objResponseModel);
                  //  return Created("Result", _objResponseModel);

                }
                else
                {
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = "Successfully Stored!";
                    _objResponseModel.APIVersion = "0.1";



                    // API Related Information  start

                    // Method mongodb

                    // API Related Information  start
                    return Ok(_objResponseModel);

                }
                
            }
            catch (Exception ex)
            {
                string errorDescription_ = ex.Message.ToString();
                _objResponseModel.StatusCode = 1;
                // _objResponseModel.ErrorDescription = errorDescription_;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                _objResponseModel.APIVersion = "0.1";
                return BadRequest( _objResponseModel);
            }



        }






    }
}
