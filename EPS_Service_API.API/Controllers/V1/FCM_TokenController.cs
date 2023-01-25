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
using static EPS_Service_API.Model.FCM_TokenModel;

namespace EPS_Service_API.API.V1.Controllers
{
    [ApiVersion("1.0")]
    // [Route("api/v{version:apiVersion}/[controller]")]

    [ApiController]
    public class FCM_TokenController : ControllerBase
    {
        private SecurityHelper _securityHelper;
        private IFCM_TokenRepository _FCM_TokenRepository;
        private readonly ILogger<BankController> _logger;

        public FCM_TokenController(
            IFCM_TokenRepository FCM_TokenRepository,
            SecurityHelper securityHelper,
            ILogger<BankController> logger
            )
        {
            _FCM_TokenRepository = FCM_TokenRepository;
            _securityHelper = securityHelper;
            _logger = logger;
        }

        [HttpPost]
        [Route("~/api/v{version:apiVersion}/FCM_Token/FCM_TokenGet")]
        public async Task<IActionResult> FCM_TokenGet([FromBody] FCM_TokenGet inputModel)
        {
            objFCM_Token _objResponseModel = new objFCM_Token();




            try
            {
                var result = await _FCM_TokenRepository.FCM_DeviceToken_Get(inputModel);

                if (result == null)
                {
                    _objResponseModel.FCM_Token = null;
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "Successful Data Load- No Data found";
                    _objResponseModel.APIVersion = "0.1";

                    //   return NotFound( _objResponseModel);
                    return Created("Result", _objResponseModel);

                }
                else
                {
                    _objResponseModel.FCM_Token = result;
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = "";
                    _objResponseModel.APIVersion = "0.1";



                    // API Related Information  start

                    // Method mongodb

                    // API Related Information  start

                    //  return Ok( _objResponseModel);
                    return Created("Result", _objResponseModel);

                }
               
            }
            catch (Exception ex)
            {
                string errorDescription_ = ex.Message.ToString();
                _objResponseModel.FCM_Token = null;
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
        [HttpPost]
            [Route("~/api/v{version:apiVersion}/FCM_Token/FCM_TokenSet")]
            public async Task<IActionResult> FCM_TokenSet([FromBody] FCM_Token inputModel)
            {
            objFCM_TokenSet _objResponseModel = new objFCM_TokenSet();




                try
                {
                    var result = await _FCM_TokenRepository.FCM_DeviceToken_Set(inputModel);

                    if (result != 1 )
                    {
                        _objResponseModel.StatusCode = 1;
                        _objResponseModel.ErrorDescription = "Device Detail was not updated! Please try Again";
                        _objResponseModel.APIVersion = "0.1";

                    //  return BadRequest( _objResponseModel);
                    return Created("Result", _objResponseModel);

                }
                    else
                    {
                        _objResponseModel.StatusCode = 200;
                        _objResponseModel.ErrorDescription = "Device Detail was updated successfully!";
                        _objResponseModel.APIVersion = "0.1";



                    // API Related Information  start

                    // Method mongodb

                    // API Related Information  start

                    // return Ok( _objResponseModel);
                    return Created("Result", _objResponseModel);

                }
                   
                }
                catch (Exception ex)
                {
                    string errorDescription_ = ex.Message.ToString();
                    _objResponseModel.StatusCode = 1;
                // _objResponseModel.ErrorDescription = errorDescription_;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                _objResponseModel.APIVersion = "0.1";
                // return BadRequest( _objResponseModel);
                return Created("Result", _objResponseModel);
            }



            }

    }
}
