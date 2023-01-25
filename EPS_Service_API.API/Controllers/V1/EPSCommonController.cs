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
using System.IO;
using System.Drawing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using AutoMapper;
using Newtonsoft.Json;

namespace EPS_Service_API.API.Controllers.V1
{
    [ApiVersion("1.0")]
    public class EPSCommonController : ControllerBase
    {
        private SecurityHelper _securityHelper;
        private IEmailSender emailSender;
        private IEmailTemplateService emailTemplateService;
        private readonly IConfiguration _config;
        private readonly ILogger<PorichoyIntegrationController> _logger;
        private readonly IMapper _mapper;
        private IPorichoyLogTaker _IPorichoyLogTaker;
        private ICustomerProfileRepository _CustomerProfileRepository;

        public EPSCommonController
          (
          SecurityHelper securityHelper,
          IEmailSender emailSender,
          IEmailTemplateService emailTemplateService,
          IConfiguration config,
          ILogger<PorichoyIntegrationController> logger,
          IMapper mapper,
          IPorichoyLogTaker PorichoyLogTaker,
          ICustomerProfileRepository CustomerProfileRepository
          )
        {
            _securityHelper = securityHelper;
            this.emailSender = emailSender;
            this.emailTemplateService = emailTemplateService;
            _config = config;
            _logger = logger;
            _mapper = mapper;
            _IPorichoyLogTaker = PorichoyLogTaker;
            _CustomerProfileRepository = CustomerProfileRepository;

        }


        [HttpPost]
        [Route("~/api/v{version:apiVersion}/Common/HelpAndFAQ")]
        public async Task<IActionResult> HelpAndFAQ([FromBody] GetCustomerProfileEntity GCPE)
        {
            string hash = Request.Headers["x-hash"].ToString(); // Check if the hash is valid

            objHelpAndFAQ _objResponseModel = new objHelpAndFAQ();



            try
            {
                var result = await _CustomerProfileRepository.EPS_Common_HelpAndFAQ_Get(GCPE.CusId);

                if (result == null)
                {
                    _objResponseModel.HelpAndFAQ = null;
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "Successful Data Load- No Data found";
                    _objResponseModel.APIVersion = "0.1";
                    //    return NotFound(_objResponseModel);
                    return Created("Result", _objResponseModel);

                }
                else
                {
                    _objResponseModel.HelpAndFAQ = result;
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = null;
                    _objResponseModel.APIVersion = "0.1";

                    // API Related Information  start

                    // Method mongodb

                    // API Related Information  start

                    //    return Ok( _objResponseModel);
                    return Created("Result", _objResponseModel);

                }
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "We caught this exception in Get Profile API.");
                string errorDescription_ = ex.Message.ToString();
                _objResponseModel.HelpAndFAQ = null;
                _objResponseModel.StatusCode = 1;
                // _objResponseModel.ErrorDescription = errorDescription_;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                _objResponseModel.APIVersion = "0.1";
                //    return BadRequest(_objResponseModel);
                return Created("Result", _objResponseModel);
            }


        }



        [HttpPost]
        [Route("~/api/v{version:apiVersion}/Common/Support")]
        public async Task<IActionResult> Support([FromBody] GetCustomerProfileEntity GCPE)
        {
            string hash = Request.Headers["x-hash"].ToString(); // Check if the hash is valid

            objSupport _objResponseModel = new objSupport();



            try
            {
                var result = await _CustomerProfileRepository.EPS_Common_Support_Get(GCPE.CusId);

                if (result == null)
                {
                    _objResponseModel.Support = null;
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "Successful Data Load- No Data found";
                    _objResponseModel.APIVersion = "0.1";
                    //    return NotFound(_objResponseModel);
                    return Created("Result", _objResponseModel);

                }
                else
                {
                    _objResponseModel.Support = result;
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = null;
                    _objResponseModel.APIVersion = "0.1";



                    // API Related Information  start

                    // Method mongodb

                    // API Related Information  start

                    //    return Ok( _objResponseModel);
                    return Created("Result", _objResponseModel);

                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "We caught this exception in Get Profile API.");
                string errorDescription_ = ex.Message.ToString();
                _objResponseModel.Support = null;
                _objResponseModel.StatusCode = 1;
                // _objResponseModel.ErrorDescription = errorDescription_;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                _objResponseModel.APIVersion = "0.1";
                //    return BadRequest( _objResponseModel);
                return Created("Result", _objResponseModel);
            }


        }



        [HttpPost]
        [Route("~/api/v{version:apiVersion}/Common/TermsAndCondition")]
        public async Task<IActionResult> TermsAndCondition([FromBody] GetCustomerProfileEntity GCPE)
        {
            string hash = Request.Headers["x-hash"].ToString(); // Check if the hash is valid

            objTermsAndCondition _objResponseModel = new objTermsAndCondition();

            try
            {
                var result = await _CustomerProfileRepository.EPS_Common_TermsAndCondition_Get(GCPE.CusId);

                if (result == null)
                {
                    _objResponseModel.TermsAndCondition = null;
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "Successful Data Load- No Data found";
                    _objResponseModel.APIVersion = "0.1";
                    //      return NotFound( _objResponseModel);
                    return Created("Result", _objResponseModel);

                }
                else
                {
                    _objResponseModel.TermsAndCondition = result;
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = null;
                    _objResponseModel.APIVersion = "0.1";



                    // API Related Information  start

                    // Method mongodb

                    // API Related Information  start

                    //   return Ok( _objResponseModel);
                    return Created("Result", _objResponseModel);

                }
              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "We caught this exception in Get Profile API.");
                string errorDescription_ = ex.Message.ToString();
                _objResponseModel.TermsAndCondition = null;
                _objResponseModel.StatusCode = 1;
                // _objResponseModel.ErrorDescription = errorDescription_;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                _objResponseModel.APIVersion = "0.1";
                //    return BadRequest( _objResponseModel);
                return Created("Result", _objResponseModel);
            }


        }


        [HttpPost]
        [Route("~/api/v{version:apiVersion}/Common/limitsettings")]
        public async Task<IActionResult> limitsettings([FromBody] GetCustomerProfileEntity GCPE)
        {


            string hash = Request.Headers["x-hash"].ToString(); // Check if the hash is valid

            objlimitsettings _objResponseModel = new objlimitsettings();

            if (ModelState.IsValid)
            {

            }
            else
            {
                // IEnumerable<string> allErrors = (IEnumerable<string>)ModelState.Values.SelectMany(v => v.Errors);
                string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
                _objResponseModel.limitsettings = null;
                _objResponseModel.StatusCode = 1;
                _objResponseModel.ErrorDescription = messages;
                //_objResponseModel.ErrorDescription = HttpStatusCode.BadRequest.ToString()+"|"+ ModelState;
                //_objResponseModel.ErrorDescription= CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                _objResponseModel.APIVersion = "0.1";
                return Created("Result", _objResponseModel);
            }



                try
            {
                var result = await _CustomerProfileRepository.EPS_Common_limitsettings_Get(GCPE.CusId);

                if (result == null)
                {
                    _objResponseModel.limitsettings = null;
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "Successful Data Load- No Data found";
                    _objResponseModel.APIVersion = "0.1";

                    //    return NotFound( _objResponseModel);
                    return Created("Result", _objResponseModel);

                }
                else
                {
                    _objResponseModel.limitsettings = result;
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = null;
                    _objResponseModel.APIVersion = "0.1";



                    // API Related Information  start

                    // Method mongodb

                    // API Related Information  start

                    //   return Ok( _objResponseModel);
                    return Created("Result", _objResponseModel);

                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "We caught this exception in Get Profile API.");
                string errorDescription_ = ex.Message.ToString();
                _objResponseModel.limitsettings = null;
                _objResponseModel.StatusCode = 1;
                // _objResponseModel.ErrorDescription = errorDescription_;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                _objResponseModel.APIVersion = "0.1";
                //   return BadRequest(_objResponseModel);
                return Created("Result", _objResponseModel);
            }


        }



       




        }
}
